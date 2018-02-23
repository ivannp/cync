using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CloudSync.Core
{
    public class OneDriveStorage : IStorage
    {
        const int RETRIES = 4;

        private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        private const string _cacheFileName = "onedrive.cache";

        private const string _clientId = "fc1e93cb-e530-4afb-b75d-aacbafdc5353";
        private const string _returnUrl = "urn:ietf:wg:oauth:2.0:oob";
        private static string[] _scopes = { "Files.ReadWrite.All" };

        private readonly string _rootPath;
        private readonly string _store;
        private TokenCache _tokenCache;
        private GraphServiceClient _graphServiceClient;
        private PublicClientApplication _clientApp;
        private AuthenticationResult _auth;

        private string TokenCachePath {
            get
            {
                return _store == null ? null : Path.Combine(_store, _cacheFileName);
            }
        }

        public OneDriveStorage(string rootPath, string store)
        {
            _rootPath = rootPath;
            _store = store;

            GetAuthenticatedClient();
        }

        public JObject ToJson()
        {
            return new JObject
            {
                ["Type"] = "OneDrive",
                ["Path"] = _rootPath,
                ["Store"] = _store
            };
        }

        public void CleanLocalFile(string path)
        {
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        public void CreateDirectory(string path)
        {
            path = LexicalPath.Clean(path);
            var folders = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var currentPath = "/";

            var parentId = _graphServiceClient.Drive.Root.Request().GetAsync().Result.Id;

            foreach (var folder in folders)
            {
                currentPath = LexicalPath.Combine(currentPath, folder);
                DriveItem item = GetItem(currentPath);
                if (item == null)
                {
                    // Create the folder
                    item = _graphServiceClient.Drive.Items[parentId].Children.Request().AddAsync(new DriveItem { Folder = new Folder(), Name = folder }).Result;
                }
                else if(item.Folder == null)
                {
                    throw new PathException($"Failed to create folder '{path}'. '{currentPath}' exists, but is a file.");
                }

                parentId = item.Id;
            }
        }

        public LocalPath Download(string src)
        {
            src = EncodePath(LexicalPath.Combine(_rootPath, src));
            LocalPath res = new LocalPath(Path.GetTempFileName());

            var downloaded = false;
            for(var i = 0; i < RETRIES; ++i)
            {
                try
                {
                    using (var stream = _graphServiceClient.Drive.Root.ItemWithPath(src).Content.Request().GetAsync().Result)
                    using (var outputStream = System.IO.File.OpenWrite(res.Path))
                        stream.CopyTo(outputStream);

                    downloaded = true;
                    break;
                }
                catch(Exception)
                {
                    LocalUtils.TryDeleteFile(res.Path);
                }
            }

            if (!downloaded)
                throw new StorageException($"Failed to download '{src}' ({RETRIES} attempts).");

            return res;
        }

        public Dictionary<string, object> GetConfig()
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(string path)
        {
            path = EncodePath(LexicalPath.Combine(_rootPath, path));
            _graphServiceClient.Drive.Root.ItemWithPath(path).Request().DeleteAsync().Wait();
        }

        public void Move(string src, string dest)
        {
            var destItem = new DriveItem { ParentReference = new ItemReference { Id = GetItem(dest).Id } };
            _graphServiceClient.Drive.Root.ItemWithPath(src).Request().UpdateAsync(destItem).Wait();
        }

        public void Upload(string src, string dest, bool finalizeLocal = true)
        {
            dest = LexicalPath.Combine(_rootPath, dest);
            var fileName = LexicalPath.GetFileName(dest);

            var item = GetItem(dest);
            if(item != null)
            {
                if(item.Folder != null)
                    throw new PathException($"Failed to create file '{dest}'. The path exists and is a directory.");
                _graphServiceClient.Drive.Items[item.Id].Request().DeleteAsync().Wait();
            }

            var fileInfo = new FileInfo(src);
            var fileSize = fileInfo.Length;

            dest = EncodePath(dest);

            var uploaded = false;
            for(var i = 0; i < RETRIES && !uploaded; ++i)
            {
                try
                {
                    using (var stream = System.IO.File.OpenRead(src))
                    {
                        if (fileSize > 1024 * 1024)
                        {
                            // TODO(ivannp) A progress indicator can be added according to:
                            //      https://github.com/OneDrive/onedrive-sdk-csharp/blob/master/docs/chunked-uploads.md, 
                            var session = _graphServiceClient.Drive.Root.ItemWithPath($"{dest}").CreateUploadSession().Request().PostAsync().Result;
                            var provider = new ChunkedUploadProvider(session, _graphServiceClient, stream);
                            item = provider.UploadAsync().Result;
                        }
                        else
                        {
                            _graphServiceClient.Drive.Root.ItemWithPath($"{dest}").Content.Request().PutAsync<DriveItem>(stream).Wait();
                        }
                    }
                    uploaded = true;
                    break;
                }
                catch(Exception)
                {
                }
            }

            if (!uploaded)
                throw new StorageException($"Failed to upload to '{dest}' ({RETRIES} attempts).");
        }

        public IEnumerable<ItemInfo> ListDirectory(string path)
        {
            path = LexicalPath.Clean(path);

            var expandValue = "children";

            var folder = _graphServiceClient.Drive.Root;
            if (path != "/")
                folder = folder.ItemWithPath(path);
            var driveItem = folder
                .Request()
                .Expand(expandValue)
                .GetAsync()
                .Result;

            var res = new List<ItemInfo>();

            foreach(var item in driveItem.Children)
                res.Add(new ItemInfo { Name = item.Name, Size = item.Size, IsDir = item.Folder != null, Id = item.Id, LastWriteTime = item.LastModifiedDateTime.Value.DateTime });

            return res;
        }

        public ItemInfo GetItemInfo(string path)
        {
            try
            {
                var item = _graphServiceClient.Drive.Root.ItemWithPath(path).Request().GetAsync().Result;
                return new ItemInfo { Name = item.Name, Size = item.Size, IsDir = item.Folder != null, Id = item.Id, LastWriteTime = item.LastModifiedDateTime.Value.DateTime };
            }
            catch(Exception)
            {
                return null;
            }
        }

        public void Download(string src, string dest)
        {
            throw new NotImplementedException();
        }

        private DriveItem GetItem(string path)
        {
            try
            {
                var item = _graphServiceClient.Drive.Root.ItemWithPath(path).Request().GetAsync().Result;
                return item;
            }
            catch
            {
                return null;
            }
        }

        private void TokenCacheAfterAccess(TokenCacheNotificationArgs args)
        {
            if (TokenCachePath == null)
                return;

            if (!args.TokenCache.HasStateChanged)
                return;

            System.IO.File.WriteAllBytes(TokenCachePath, args.TokenCache.Serialize());
            args.TokenCache.HasStateChanged = false;
        }

        private void TokenCacheBeforeAccess(TokenCacheNotificationArgs args)
        {
            if (TokenCachePath == null)
                return;

            if (!System.IO.File.Exists(TokenCachePath))
                return;

            args.TokenCache.Deserialize(System.IO.File.ReadAllBytes(TokenCachePath));
        }

        private void GetAuthenticatedClient()
        {
            if (_graphServiceClient == null)
            {
                //_tokenCache = new TokenCache();
                //if (System.IO.File.Exists(TokenCachePath))
                //    _tokenCache.Deserialize(System.IO.File.ReadAllBytes(TokenCachePath));
                //_tokenCache.SetAfterAccess(TokenCacheAfterAccess);
                //_tokenCache.SetBeforeAccess(TokenCacheBeforeAccess);

                //_clientApp = new PublicClientApplication(_clientId, "https://login.microsoftonline.com/common", _tokenCache);
                _clientApp = new PublicClientApplication(_clientId);

                // Create Microsoft Graph client.
                try
                {
                    _graphServiceClient = new GraphServiceClient(
                        "https://graph.microsoft.com/v1.0",
                        new DelegateAuthenticationProvider(
                            async (requestMessage) =>
                            {
                                var token = await GetUserTokenAsync();
                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

                            }));
                }
                catch (Exception ex)
                {
                    _logger.Error($"Could not create a graph client: {ex.Message}");
                }
            }
        }

        private async Task<string> GetUserTokenAsync()
        {
            bool retry = true;
            if(_auth != null && _auth.User != null)
            {
                try
                {
                    _auth = await _clientApp.AcquireTokenSilentAsync(_scopes, _auth.User);
                    retry = false;
                }
                catch (Exception)
                {
                }
            }

            if(retry)
            {
                if (_auth == null || _auth.User == null || _auth.ExpiresOn <= DateTimeOffset.UtcNow.AddMinutes(5))
                {
                    _auth = await _clientApp.AcquireTokenAsync(_scopes);
                }
            }

            return _auth.AccessToken;
        }

        private void SaveToken()
        {
            var jo = new JObject
            {
                ["AccessToken"] = _auth.AccessToken,
                ["ExpiresOn"] = _auth.ExpiresOn.ToUnixTimeMilliseconds(),
                ["UserIdentifier"] = _auth.User.Identifier,
                ["UserName"] = _auth.User.Name
            };
            System.IO.File.WriteAllText(_store, jo.ToString());
        }

        private void LoadToken()
        {
        }

        private string EncodePath(string path)
        {
            return (path[0] == '/' ? "/" : "") + string.Join("/", path.Split('/').Select(p => Uri.EscapeDataString(p)).ToList());
        }
    }
}

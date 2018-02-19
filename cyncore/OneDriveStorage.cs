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
        private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        private const string _clientId = "d2356b67-dd6a-4163-9fae-df91859383ab";
        private const string _returnUrl = "urn:ietf:wg:oauth:2.0:oob";
        private static string[] _scopes = { "Files.ReadWrite.All" };

        private readonly string _rootPath;
        private readonly string _store;
        private GraphServiceClient _graphServiceClient;
        private PublicClientApplication _clientApp = new PublicClientApplication(_clientId);
        private AuthenticationResult _auth;

        private Dictionary<string, string> _folderIdCache;

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

        public FileInfo DefaultFileInfo(bool dir = true)
        {
            throw new NotImplementedException();
        }

        public LocalPath Download(string src)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetConfig()
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(string path)
        {
            throw new NotImplementedException();
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
                _graphServiceClient.Drive.Items[item.Id].Request().DeleteAsync().RunSynchronously();
            }

            using (var stream = System.IO.File.OpenRead(src))
            {
                // TODO(ivannp) A progress indicator can be added according to:
                //      https://github.com/OneDrive/onedrive-sdk-csharp/blob/master/docs/chunked-uploads.md, 
                var session = _graphServiceClient.Drive.Root.ItemWithPath($"{dest}").CreateUploadSession().Request().PostAsync().Result;
                var provider = new ChunkedUploadProvider(session, _graphServiceClient, stream);
                item = provider.UploadAsync().Result;
            }
        }

        public IEnumerable<ItemInfo> ListDirectory(string path)
        {
            path = LexicalPath.Clean(path);

            DriveItem folder;

            var expandValue = "children";

            folder = _graphServiceClient.Drive.Root
                .ItemWithPath(path)
                .Request()
                .Expand(expandValue)
                .GetAsync()
                .Result;

            var res = new List<ItemInfo>();

            foreach(var item in folder.Children)
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

        private void GetAuthenticatedClient()
        {
            if (_graphServiceClient == null)
            {
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
    }
}

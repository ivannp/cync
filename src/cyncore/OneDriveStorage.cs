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
        private const string _secret = "aVABQ48045$?$zawvosZBA~";

        private readonly string _rootPath;
        private readonly string _store;
        private AzureAuthenticationProvider _authenticationProvider;
        private GraphServiceClient _graphServiceClient;

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

            GetGraphClient();
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
            var res = new LocalPath(LocalUtils.GetTempFileName());

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
            // TODO(ivannp) Increase the page list to max. Not sure what the max is. :)
            path = LexicalPath.Combine(_rootPath, LexicalPath.Clean(path));

            var folder = _graphServiceClient.Drive.Root;
            if (path != "/")
                folder = folder.ItemWithPath(path);

            var res = new List<ItemInfo>();

            var collection = folder
                .Children
                .Request()
                .GetAsync()
                .Result;

            while (true)
            {
                foreach (var item in collection.CurrentPage)
                    res.Add(new ItemInfo { Name = item.Name, Size = item.Size, IsDir = item.Folder != null, Id = item.Id, LastWriteTime = item.LastModifiedDateTime.Value.DateTime });

                if (collection.NextPageRequest == null)
                    break;

                collection = collection.NextPageRequest.GetAsync().Result;
            }

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

        private void GetGraphClient()
        {
            _authenticationProvider = new AzureAuthenticationProvider(_clientId, _secret, "common", _store);
            _graphServiceClient = new GraphServiceClient(_authenticationProvider);
        }

        private string EncodePath(string path)
        {
            return (path[0] == '/' ? "/" : "") + string.Join("/", path.Split('/').Select(p => Uri.EscapeDataString(p)).ToList());
        }
    }
}

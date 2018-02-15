using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CloudSync.Core
{
    public class GoogleDriveStorage : IStorage
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly string _rootPath;
        private readonly string _store;

        private DriveService service;
        private Dictionary<string, string> folderId;

        public void CleanLocalFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public void CreateDirectory(string path)
        {
            path = LexicalPath.Combine(_rootPath, LexicalPath.Clean(path));
            var parentId = "root";
            var folders = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var currentPath = "";
            bool skipCheck = false; // Set to true the first time we create a directory
            foreach (var folder in folders)
            {
                currentPath = LexicalPath.Combine(currentPath, folder);

                string id;
                var success = folderId.TryGetValue(currentPath, out id);
                if(success)
                {
                    // The folder exists
                    parentId = id;
                    continue;
                }

                IList<Google.Apis.Drive.v3.Data.File> files = null;
                if (!skipCheck)
                {
                    var req = service.Files.List();
                    req.PageSize = 10;  // We expect a single result
                    req.Fields = "files(id)";
                    // req.Fields = "files(*)";
                    req.Spaces = "drive";
                    req.Q = $"'{parentId}' in parents and name = '{folder}' and mimeType = 'application/vnd.google-apps.folder' and trashed = false";

                    files = req.Execute().Files;
                }

                if (files?.Count > 0)
                {
                    parentId = files[0].Id;
                }
                else
                {
                    var file = new Google.Apis.Drive.v3.Data.File();
                    file.Name = folder;
                    file.MimeType = "application/vnd.google-apps.folder";
                    file.Parents = new string[] { $"{parentId}" };
                    var createReq = service.Files.Create(file);
                    // createReq.Fields = "id";
                    parentId = createReq.Execute().Id;

                    skipCheck = true;
                }

                folderId.Add(currentPath, parentId);
            }
        }

        public IEnumerable<ItemInfo> ListDirectory(string path)
        {
            var res = new List<ItemInfo>();

            var id = GetId(path);

            var req = service.Files.List();
            req.Fields = "files(id, name, mimeType, size, modifiedTime)";
            req.Spaces = "drive";
            req.Q = $"'{id}' in parents and trashed = false";
            // TODO(ivannp): Support folders with more than 1000 entries
            req.PageSize = 1000;

            var files = req.Execute().Files;
            if(files == null || files.Count == 0)
                return res;

            foreach(var file in files)
                res.Add(new ItemInfo { Name = file.Name, Size = file.Size, IsDir = file.MimeType == "application/vnd.google-apps.folder", Id = file.Id, LastWriteTime = file.ModifiedTime });

            return res;
        }

        public ItemInfo GetItemInfo(string path)
        {
            var folder = LexicalPath.GetDirectoryName(path);

            string id = GetId(folder, exception: false);
            if (id == null)
                return null;

            var fileName = LexicalPath.GetFileName(path);

            var req = service.Files.List();
            req.PageSize = 10; // We expect 0 or 1 elements
            req.Fields = "files(id, name, mimeType, size, modifiedTime)";
            req.Spaces = "drive";
            req.Q = $"name = '{fileName}' and '{id}' in parents and trashed = false";

            var files = req.Execute().Files;
            if (files.Count > 1)
                throw new Exception($"{files.Count} files returned, expected one.");
            if (files.Count == 0)
                return null;
            var file = files[0];
            return new ItemInfo { Name = file.Name, Size = file.Size, IsDir = file.MimeType == "application/vnd.google-apps.folder", Id = file.Id, LastWriteTime = file.ModifiedTime };
        }

        public FileInfo DefaultFileInfo(bool dir = true)
        {
            throw new NotImplementedException();
        }

        public LocalPath Download(string src)
        {
            src = LexicalPath.Combine(_rootPath, src);
            string id = GetId(src);
            var req = service.Files.Get(id);
            LocalPath res = new LocalPath(Path.GetTempFileName());
            using (var stream = File.OpenWrite(res.Path))
            {
                req.Download(stream);
            }
            return res;
        }

        public void Download(string src, string dest)
        {
            src = LexicalPath.Combine(_rootPath, src);
            string id = GetId(src);
            var req = service.Files.Get(id);
            using (var stream = File.OpenWrite(dest))
                req.Download(stream);
        }

        public void RemoveFile(string path)
        {
            path = LexicalPath.Combine(_rootPath, path);
            string id = GetId(path);
            var req = service.Files.Delete(id);
            req.Execute();

            folderId.Remove(path);
        }

        public void Upload(string src, string dest, bool finalizeLocal = true)
        {
            dest = LexicalPath.Combine(_rootPath, dest);
            var folder = LexicalPath.GetDirectoryName(dest);
            string id = GetId(folder);
            var fileName = LexicalPath.GetFileName(dest);

            // Does the file exist?
            var req = service.Files.List();
            req.PageSize = 10;  // We expect a single result
            req.Fields = "files(id, name, mimeType)";
            req.Spaces = "drive";
            req.Q = $"'{id}' in parents and name = '{fileName}' and trashed = false";

            var files = req.Execute().Files;
            if (files?.Count > 0)
            {
                if (files.Count > 1) _logger.Warn($"{files.Count} files returned, expected one.");

                // Update an existing file
                using (var stream = new FileStream(src, FileMode.Open))
                {
                    var fi = new FileInfo(src);
                    var updateRequest = service.Files.Update(new Google.Apis.Drive.v3.Data.File(), files[0].Id, stream, "application/octet-stream");
                    var response = updateRequest.Upload();
                }
            }
            else
            {
                // Create a new file
                var file = new Google.Apis.Drive.v3.Data.File();
                file.Name = LexicalPath.GetFileName(dest);
                file.Parents = new string[] { $"{id}" };
                using (var stream = new FileStream(src, FileMode.Open))
                {
                    var createRequest = service.Files.Create(file, stream, "application/octet-stream");
                    createRequest.Fields = "id";
                    var resFile = createRequest.Upload();
                }
            }

            if (finalizeLocal) File.Delete(src);
        }

        public string GetId(string path, bool exception = true)
        {
            path = LexicalPath.Clean(path);
            string id;
            var success = folderId.TryGetValue(path, out id);
            if (success) return id;

            var parentId = "root";
            var folders = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var currentPath = "";
            foreach(var folder in folders)
            {
                currentPath = LexicalPath.Combine(currentPath, folder);
                success = folderId.TryGetValue(currentPath, out id);
                if(success)
                {
                    parentId = id;
                    continue;
                }

                var req = service.Files.List();
                req.PageSize = 10;  // We expect a single result
                req.Fields = "files(id, name)";
                req.Spaces = "drive";
                req.Q = $"'{parentId}' in parents and name = '{folder}' and trashed = false";

                var files = req.Execute().Files;

                if(files?.Count > 0)
                {
                    parentId = files[0].Id;
                }
                else
                {
                    if (exception)
                        throw new PathException($"Failed to open {path}.");
                    else
                        return null;
                }

                folderId.Add(currentPath, parentId);
            }
            return parentId;
        }

        public JObject ToJson()
        {
            return new JObject
            {
                ["Type"] = "GoogleDrive",
                ["Path"] = _rootPath,
                ["Store"] = _store
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="rootPath">The root path of the repository.</param>
        /// <param name="store">The path where the token is cached and/or refreshed to</param>
        public GoogleDriveStorage(string rootPath, string store)
        {
            _rootPath = rootPath;
            _store = store;

            folderId = new Dictionary<string, string>();

            UserCredential credential;

            string[] Scopes = { DriveService.Scope.Drive };
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            using (var ss = new FileStream(Path.Combine(exePath, "secret.json"), FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(ss).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(store, true)).Result;
            }

            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "sot",
            });
        }
    }
}

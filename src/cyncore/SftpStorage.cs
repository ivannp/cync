using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace CloudSync.Core
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SftpStorage : IStorage
    {
        private const int DefaultPort = 22;

        [JsonProperty(PropertyName = "RootPath", Required = Required.Always)]
        private readonly string _rootPath;

        [JsonProperty(PropertyName = "User", Required = Required.Always)]
        private readonly string _user;

        [JsonProperty(PropertyName = "Password")]
        private readonly string _password;

        [JsonProperty(PropertyName = "KeyPath")]
        private readonly string _keyPath;

        [JsonProperty(PropertyName = "Host", Required = Required.Always)]
        private readonly string _host;

        [JsonProperty(PropertyName = "Port")]
        private readonly int _port;
        
        private readonly SftpClient _client;

        public SftpStorage(string user, string password, string keyPath, string host, int? port = null, string rootPath = "", bool createRoot = false)
        {
            _rootPath = rootPath;
            _user = user;
            _password = password;
            _keyPath = keyPath;
            _host = host;
            _port = port ?? DefaultPort;

            ConnectionInfo connectionInfo = null;
            if (keyPath != null)
                connectionInfo = new PrivateKeyConnectionInfo(host, _port, _user, new PrivateKeyFile(keyPath));
            else
                connectionInfo = new PasswordConnectionInfo(host, _port, user, password);
            _client = new SftpClient(connectionInfo);
            _client.Connect();

            if (createRoot)
            {
                try
                {
                    _client.CreateDirectory(_rootPath);
                }
                catch(Exception)
                {
                }
            }
        }

        public void CleanLocalFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public void CreateDirectory(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                path = LexicalPath.Combine(_rootPath, path);
            TryCreateDirectory(path);
        }

        public LocalPath Download(string src)
        {
            var path = string.IsNullOrWhiteSpace(_rootPath) ? src : LexicalPath.Combine(_rootPath, src);
            LocalPath res = new LocalPath(LocalUtils.GetTempFileName());
            using (var stream = File.OpenWrite(res.Path))
                _client.DownloadFile(path, stream);
            return res;
        }

        public void Download(string src, string dest)
        {
            var path = string.IsNullOrWhiteSpace(_rootPath) ? src : LexicalPath.Combine(_rootPath, src);
            using (var stream = new FileStream(dest, FileMode.OpenOrCreate))
            {
                stream.SetLength(0);
                _client.DownloadFile(path, stream);
            }
        }

        public ItemInfo GetItemInfo(string path)
        {
            if(!string.IsNullOrWhiteSpace(_rootPath))
                path = LexicalPath.Combine(_rootPath, path);

            var info = _client.Get(path);
            return new ItemInfo { Name = info.Name, Size = info.Length, IsDir = info.IsDirectory, Id = info.FullName, LastWriteTime = info.LastWriteTime };
        }

        public IEnumerable<ItemInfo> ListDirectory(string path)
        {
            if (!string.IsNullOrWhiteSpace(_rootPath))
                path = LexicalPath.Combine(_rootPath, path);

            var res = new List<ItemInfo>();
            foreach(var info in _client.ListDirectory(path))
            {
                if (info.Name == "." || info.Name == "..")
                    continue;
                res.Add(new ItemInfo { Name = info.Name, Size = info.Length, IsDir = info.IsDirectory, Id = info.FullName, LastWriteTime = info.LastWriteTime });
            }
                
            return res;
        }

        public void Move(string src, string dest)
        {
            throw new NotSupportedException($"The 'move' command is not supported.");
        }

        public void RemoveFile(string path)
        {
            if (!string.IsNullOrWhiteSpace(_rootPath))
                path = LexicalPath.Combine(_rootPath, path);
            _client.DeleteFile(path);
        }

        public JObject ToJson()
        {
            return new JObject
            {
                ["Type"] = "Sftp",
                ["Path"] = _rootPath,
                ["User"] = _user,
                ["Password"] = _password,
                ["KeyPath"] = _keyPath,
                ["Host"] = _host,
                ["Port"] = _port
            };
        }

        public void Upload(string src, string dest, bool finalizeLocal = true)
        {
            var path = string.IsNullOrWhiteSpace(_rootPath) ? dest : LexicalPath.Combine(_rootPath, dest);
            using (var stream = new FileStream(src, FileMode.Open))
                _client.UploadFile(stream, path);
            if (finalizeLocal)
                File.Delete(src);
        }

        private bool TryCreateDirectory(string path)
        {
            try
            {
                _client.CreateDirectory(path);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}

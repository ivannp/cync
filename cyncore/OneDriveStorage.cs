using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
            throw new NotImplementedException();
        }

        public void CreateDirectory(string path)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Download(string src, string dest)
        {
            throw new NotImplementedException();
        }

        private class AuthenticationProvider : IAuthenticationProvider
        {
            public Task AuthenticateRequestAsync(HttpRequestMessage request)
            {
                throw new NotImplementedException();
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
            if(_auth == null || _auth.User == null || _auth.ExpiresOn <= DateTimeOffset.UtcNow.AddMinutes(5))
            {
                _auth = await _clientApp.AcquireTokenAsync(_scopes);
            }

            return _auth.AccessToken;
        }
    }
}

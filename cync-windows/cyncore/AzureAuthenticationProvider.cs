using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudSync.Core
{
    class AzureAuthenticationProvider : IAuthenticationProvider
    {
        public const string BaseAuthenticationUrl = "https://login.microsoftonline.com/";
        public const string ResourceUrl = "https://graph.microsoft.com";

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tenant;
        private static string[] _scopes = { "Files.ReadWrite.All" };

        private const string _cacheFileName = "onedrive.cache";

        private TokenCache _tokenCache;

        private string TokenCachePath { get; set; }

        private string AuthenticationUrl => BaseAuthenticationUrl + _tenant;

        public PublicClientApplication ClientApp { get; }

        public AzureAuthenticationProvider(string clientId, string clientSecret, string tenant, string store)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tenant = tenant;

            TokenCachePath = Path.Combine(store, _cacheFileName);

            _tokenCache = new TokenCache();

            if (System.IO.File.Exists(TokenCachePath))
                _tokenCache.Deserialize(System.IO.File.ReadAllBytes(TokenCachePath));
            _tokenCache.SetAfterAccess(TokenCacheAfterAccess);

            ClientApp = new PublicClientApplication(_clientId, "https://login.microsoftonline.com/common", _tokenCache);
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

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var retry = true;
            AuthenticationResult auth = null;
            try
            {
                auth = await ClientApp.AcquireTokenSilentAsync(_scopes, ClientApp.Users.FirstOrDefault());
                retry = false;
            }
            catch (Exception)
            {

            }

            if (retry)
                auth = await ClientApp.AcquireTokenAsync(_scopes);
            request.Headers.Add("Authorization", "Bearer " + auth.AccessToken);
        }
    }
}


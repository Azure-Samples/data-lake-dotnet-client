using System;
using Microsoft.Rest;
using MSAD = Microsoft.IdentityModel.Clients.ActiveDirectory;
using REST = Microsoft.Rest.Azure;

namespace AdlClient
{
    public class InteractiveAuthentication: AuthenticationBase
    {
        public InteractiveAuthentication(string tenant) : base(tenant)
        {
            // Re-use the Azure PowerShell client id, in production code you should create your own client id
            this.ClientID = "1950a258-227b-4e31-a9cf-717495945fc2"; 
        }

        public void ClearCache()
        {
            string cache_filename = GetTokenCachePath();
            
            if (System.IO.File.Exists(cache_filename))
            {
                var bytes = System.IO.File.ReadAllBytes(cache_filename);
                var token_cache = new MSAD.TokenCache(bytes);
                token_cache.Clear();
                System.IO.File.WriteAllBytes(cache_filename, token_cache.Serialize());
            }
        }

        private static ServiceClientCredentials GetCreds_User_Popup(
            string tenant,
            System.Uri tokenAudience,
            string clientId,
            MSAD.TokenCache tokenCache,
            MSAD.PromptBehavior promptBehavior = MSAD.PromptBehavior.Auto)
        {
            System.Threading.SynchronizationContext.SetSynchronizationContext(new System.Threading.SynchronizationContext());

            var clientSettings = new REST.Authentication.ActiveDirectoryClientSettings
            {
                ClientId = clientId,
                ClientRedirectUri = new System.Uri("urn:ietf:wg:oauth:2.0:oob"),
                PromptBehavior = promptBehavior
            };

            var serviceSettings = REST.Authentication.ActiveDirectoryServiceSettings.Azure;
            serviceSettings.TokenAudience = tokenAudience;

            var creds = REST.Authentication.UserTokenProvider.LoginWithPromptAsync(
                tenant,
                clientSettings,
                serviceSettings,
                tokenCache).GetAwaiter().GetResult();
            return creds;
        }

        private static MSAD.TokenCache GetTokenCache(string path)
        {
            var tokenCache = new MSAD.TokenCache();

            tokenCache.BeforeAccess += notificationArgs =>
            {
                if (System.IO.File.Exists(path))
                {
                    var bytes = System.IO.File.ReadAllBytes(path);
                    notificationArgs.TokenCache.Deserialize(bytes);
                }
            };

            tokenCache.AfterAccess += notificationArgs =>
            {
                var bytes = notificationArgs.TokenCache.Serialize();
                System.IO.File.WriteAllBytes(path, bytes);
            };
            return tokenCache;
        }

        public override void _Authenticate()
        {
            var tokenCache = GetTokenCache(this.GetTokenCachePath());

            this.ArmCreds = GetCreds_User_Popup(this.Tenant, this.ArmTokenAudience, this.ClientID, tokenCache);
            this.AdlCreds = GetCreds_User_Popup(this.Tenant, this.AdlTokenAudience, this.ClientID, tokenCache);
            this.GraphCreds = GetCreds_User_Popup(this.Tenant, this.GraphTokenAudience, this.ClientID, tokenCache);
        }


        private string GetTokenCachePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string basefname = "AzureDataLakeClient_[" + this.Tenant + "].tokencache";
            var tokenCachePath = System.IO.Path.Combine(path, basefname);
            return tokenCachePath;
        }
    }
}
 
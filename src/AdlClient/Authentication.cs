using System;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using MSAD = Microsoft.IdentityModel.Clients.ActiveDirectory;
using REST = Microsoft.Rest.Azure;

namespace AdlClient
{
    public class Authentication
    {
        public readonly string Tenant;
        private MSAD.TokenCache _tokenCache;

        public Microsoft.Rest.ServiceClientCredentials ARMCreds;
        public Microsoft.Rest.ServiceClientCredentials ADLCreds;
        public Microsoft.Rest.ServiceClientCredentials AADCreds;


        public Authentication(string tenant)
        {
            this.Tenant = tenant;
        }

        public void ClearCache()
        {
            string cache_filename = GetTokenCachePath();
            
            if (System.IO.File.Exists(cache_filename))
            {
                var bytes = System.IO.File.ReadAllBytes(cache_filename);
                var token_cache = new Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache(bytes);
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

        public void Authenticate()
        {
            string CLIENTID = "1950a258-227b-4e31-a9cf-717495945fc2"; // Re-use the Azure PowerShell client id, in production code you should create your own client id
            var ARM_TOKEN_AUDIENCE = new System.Uri(@"https://management.core.windows.net/");
            var ADL_TOKEN_AUDIENCE = new System.Uri(@"https://datalake.azure.net/");
            var AAD_TOKEN_AUDIENCE = new System.Uri(@"https://graph.windows.net/");

            var tokenCache = GetTokenCache(this.GetTokenCachePath());
            this.ARMCreds = GetCreds_User_Popup(this.Tenant, ARM_TOKEN_AUDIENCE, CLIENTID, tokenCache);
            this.ADLCreds = GetCreds_User_Popup(this.Tenant, ADL_TOKEN_AUDIENCE, CLIENTID, tokenCache);
            this.AADCreds = GetCreds_User_Popup(this.Tenant, AAD_TOKEN_AUDIENCE, CLIENTID, tokenCache);
        }

        private string GetTokenCachePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string basefname = "AzureDataLakeClient_[" + this.Tenant + "].tokencache";
            var tokenCachePath = System.IO.Path.Combine(path, basefname);
            return tokenCachePath;
        }

        public static string GetTenantId(string tenant)
        {
            // example https://login.windows.net/microsoft.onmicrosoft.com/.well-known/openid-configuration
            string url = "https://login.windows.net/" + tenant + "/.well-known/openid-configuration";

            var wc = new System.Net.WebClient();
            var s = wc.OpenRead(url);
            string result;
            using (var reader = new System.IO.StreamReader(s))
            {
                result = reader.ReadToEnd();
            }
            var root = JObject.Parse(result);
            var token_endpoint_element = root["token_endpoint"];
            string token_endpoint = token_endpoint_element.Value<string>();

            var parts = token_endpoint.Split('/');

            string tenantid = parts[3];
            return tenantid;
        }
    }

}
 
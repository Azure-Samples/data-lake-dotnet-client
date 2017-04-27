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
        private Microsoft.Rest.ServiceClientCredentials _service_client_creds;
        private MSAD.TokenCacheItem _token;

        public ServiceClientCredentials ServiceClientCredentials {
            get => _service_client_creds;
            private set => _service_client_creds = value; }

        public MSAD.TokenCacheItem TokenCacheItem {
            get => _token;
            private set => _token = value; }

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

        public void Authenticate()
        {

            string domain = this.Tenant; // if you want it to automatically use a tenant use "common" - but this can pick the an unintended tenant so it is best to be explicit
            string client_id = "1950a258-227b-4e31-a9cf-717495945fc2"; // Re-use the Azure PowerShell client id, in production code you should create your own client id

            var client_redirect = new System.Uri("urn:ietf:wg:oauth:2.0:oob");
            var AD_client_settings = REST.Authentication.ActiveDirectoryClientSettings.UseCacheCookiesOrPrompt(client_id, client_redirect);

            // Load the token cache, if one exists.
            string cache_filename = GetTokenCachePath();

            MSAD.TokenCache token_cache;

            if (System.IO.File.Exists(cache_filename))
            {
                var bytes = System.IO.File.ReadAllBytes(cache_filename);
                token_cache = new MSAD.TokenCache(bytes);
            }
            else
            {
                token_cache = new MSAD.TokenCache();
            }

            // Did not find the token in the cache, show popup and save the token
            var sync_context = new System.Threading.SynchronizationContext();
            System.Threading.SynchronizationContext.SetSynchronizationContext(sync_context);
            var scv_client_creds = REST.Authentication.UserTokenProvider.LoginWithPromptAsync(domain, AD_client_settings, token_cache).Result;

            // serialization only works if there is more then one item in the cache
            if (token_cache.Count > 0)
            {
                System.IO.File.WriteAllBytes(cache_filename, token_cache.Serialize());
            }

            this.ServiceClientCredentials = scv_client_creds;
            this.TokenCacheItem = token_cache.ReadItems().First();
        }

        private static void SaveTokenCache(MSAD.TokenCache token_cache, string filename)
        {
            var bytes = token_cache.Serialize();
            System.IO.File.WriteAllBytes(filename, bytes);
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
                result = reader.ReadToEnd(); // do something fun...                    |
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
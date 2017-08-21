using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using MSAD = Microsoft.IdentityModel.Clients.ActiveDirectory;
using REST = Microsoft.Rest.Azure;

namespace AdlClient
{
    public class NonInteractiveAuthentication : AuthenticationBase
    {
        public string SecretKey;
        public X509Certificate2 Certificate;

        public NonInteractiveAuthentication(string tenant, string clientid, string secretkey) : base(tenant)
        {
            this.ClientID = clientid;
            this.SecretKey = secretkey;
            this.Certificate = null;
        }

        public NonInteractiveAuthentication(string tenant, string clientid, X509Certificate2 cert) : base(tenant)
        {
            this.ClientID = clientid;
            this.SecretKey = null;
            this.Certificate = cert;
        }

        public override void _Authenticate()
        {
            if (this.SecretKey != null)
            {
                this.ArmCreds = GetCredsServicePrincipalSecretKey(this.Tenant, this.ArmTokenAudience, this.ClientID, this.SecretKey);
                this.AdlCreds = GetCredsServicePrincipalSecretKey(this.Tenant, this.ArmTokenAudience, this.ClientID, this.SecretKey);
                this.AadCreds = GetCredsServicePrincipalSecretKey(this.Tenant, this.ArmTokenAudience, this.ClientID, this.SecretKey);
            }
            else
            {
                this.ArmCreds = GetCredsServicePrincipalCertificate(this.Tenant, this.ArmTokenAudience, this.ClientID, this.Certificate);
                this.AdlCreds = GetCredsServicePrincipalCertificate(this.Tenant, this.ArmTokenAudience, this.ClientID, this.Certificate);
                this.AadCreds = GetCredsServicePrincipalCertificate(this.Tenant, this.ArmTokenAudience, this.ClientID, this.Certificate);

            }
        }

        private static ServiceClientCredentials GetCredsServicePrincipalSecretKey(string domain, Uri tokenAudience, string clientId, string secretKey)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var serviceSettings = ActiveDirectoryServiceSettings.Azure;
            serviceSettings.TokenAudience = tokenAudience;

            var creds = ApplicationTokenProvider.LoginSilentAsync(domain, clientId, secretKey, serviceSettings).GetAwaiter().GetResult();

            return creds;
        }

        private static ServiceClientCredentials GetCredsServicePrincipalCertificate(string domain, Uri tokenAudience, string clientId, X509Certificate2 certificate)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var clientAssertionCertificate = new MSAD.ClientAssertionCertificate(clientId, certificate);
            var serviceSettings = ActiveDirectoryServiceSettings.Azure;
            serviceSettings.TokenAudience = tokenAudience;

            var creds = ApplicationTokenProvider.LoginSilentWithCertificateAsync(domain, clientAssertionCertificate, serviceSettings).GetAwaiter().GetResult();

            return creds;
        }
    }


    public class InteractiveAuthentication: AuthenticationBase
    {
        private MSAD.TokenCache _tokenCache;

        public InteractiveAuthentication(string tenant) : base(tenant)
        {
            this.ClientID = "1950a258-227b-4e31-a9cf-717495945fc2"; // Re-use the Azure PowerShell client id, in production code you should create your own client id
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
            this.AadCreds = GetCreds_User_Popup(this.Tenant, this.AadTokenAudience, this.ClientID, tokenCache);
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
 
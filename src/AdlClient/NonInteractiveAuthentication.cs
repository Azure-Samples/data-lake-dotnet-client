using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;

namespace AdlClient
{
    public class NonInteractiveAuthentication : AuthenticationBase
    {
        public readonly string SecretKey;
        public readonly X509Certificate2 Certificate;

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
                this.GraphCreds = GetCredsServicePrincipalSecretKey(this.Tenant, this.ArmTokenAudience, this.ClientID, this.SecretKey);
            }
            else
            {
                this.ArmCreds = GetCredsServicePrincipalCertificate(this.Tenant, this.ArmTokenAudience, this.ClientID, this.Certificate);
                this.AdlCreds = GetCredsServicePrincipalCertificate(this.Tenant, this.ArmTokenAudience, this.ClientID, this.Certificate);
                this.GraphCreds = GetCredsServicePrincipalCertificate(this.Tenant, this.ArmTokenAudience, this.ClientID, this.Certificate);

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

            var clientAssertionCertificate = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientAssertionCertificate(clientId, certificate);
            var serviceSettings = ActiveDirectoryServiceSettings.Azure;
            serviceSettings.TokenAudience = tokenAudience;

            var creds = ApplicationTokenProvider.LoginSilentWithCertificateAsync(domain, clientAssertionCertificate, serviceSettings).GetAwaiter().GetResult();

            return creds;
        }
    }
}
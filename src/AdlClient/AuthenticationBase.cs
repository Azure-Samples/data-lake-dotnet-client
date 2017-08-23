using Newtonsoft.Json.Linq;

namespace AdlClient
{
    public abstract class AuthenticationBase
    {
        // information needed to get the credentials
        public readonly string Tenant;
        readonly public System.Uri ArmTokenAudience = new System.Uri(@"https://management.core.windows.net/");
        readonly public System.Uri AdlTokenAudience = new System.Uri(@"https://datalake.azure.net/");
        readonly public System.Uri AadTokenAudience = new System.Uri(@"https://graph.windows.net/");
        public string ClientID;

        // credentials will be stored here
        public Microsoft.Rest.ServiceClientCredentials ArmCreds;
        public Microsoft.Rest.ServiceClientCredentials AdlCreds;
        public Microsoft.Rest.ServiceClientCredentials AadCreds;
        
        protected AuthenticationBase(string tenant)
        {
            this.Tenant = tenant;
        }

        public void Authenticate()
        {
            this._Authenticate();
        }

        public abstract void _Authenticate();

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
 
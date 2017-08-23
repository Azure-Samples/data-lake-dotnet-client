using MSAZURERM = Microsoft.Azure.Management.ResourceManager;
using System.Collections.Generic;
using Microsoft.Azure.Management.ResourceManager; // Needed for extension methods
using System.Linq;

namespace AdlClient
{
    public class AzureClient: ClientBase
    {
        public readonly AdlClient.Commands.AnalyticsRmCommands Analytics;
        public readonly AdlClient.Commands.StoreRmCommands Store;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient GraphClient;

        public AzureClient(InteractiveAuthentication auth) :
            base(auth)
        {
            this.Analytics = new AdlClient.Commands.AnalyticsRmCommands(auth);
            this.Store = new AdlClient.Commands.StoreRmCommands(auth);
            this.GraphClient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(auth.GraphCreds);
            this.GraphClient.TenantID = auth.Tenant;
        }

        public IEnumerable<MSAZURERM.Models.Subscription> ListSubscriptions()
        {
            var sub_client = new MSAZURERM.SubscriptionClient(this.Authentication.ArmCreds);

            var subs = sub_client.Subscriptions.List();
            foreach (var sub in subs)
            {
                yield return sub;
            }
        }

        // ----------
        public IEnumerable<MSAZURERM.Models.ResourceGroup> ListResourceGroups(string subid)
        {
            var rm_client = new MSAZURERM.ResourceManagementClient(this.Authentication.ArmCreds);
            rm_client.SubscriptionId = subid;

            var rgs = rm_client.ResourceGroups.List();
            foreach (var rg in rgs)
            {
                yield return rg;
            }
        }


        public static string GetTenantIdFromName(string name)
        {
            string url = "https://login.windows.net/" + name + "/.well-known/openid-configuration";

            var request = System.Net.WebRequest.Create(url);

            ((System.Net.HttpWebRequest)request).UserAgent = "Mozilla/5.0 (Windows NT; Windows NT 10.0; en-US) WindowsPowerShell/5.1.14393.0";


            var response = (System.Net.HttpWebResponse)request.GetResponse();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.ArgumentException();
            }

            var dataStream = response.GetResponseStream();

            var reader = new System.IO.StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();


            reader.Close();
            dataStream.Close();
            response.Close();

            if (response.Headers["Content-Type"] == "text/html")
            {
                throw new System.ArgumentException();
            }

            var j0 = Newtonsoft.Json.Linq.JObject.Parse(responseFromServer);

            var j1 = j0.SelectToken("authorization_endpoint");
            var j2 = j0.SelectToken("token_endpoint");

            var tokens = j2.ToString().Split('/');

            string TenantId = tokens[3];

            return TenantId;
        }
    }
}

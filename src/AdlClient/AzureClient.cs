using MSAZURERM = Microsoft.Azure.Management.ResourceManager;
using System.Collections.Generic;
using Microsoft.Azure.Management.ResourceManager; // Needed for extension methods

namespace AdlClient
{
    public class AzureClient: ClientBase
    {
        private readonly AdlClient.Rest.AnalyticsAccountManagmentRestWrapper _adlaAccountMgmtClientWrapper;
        private readonly AdlClient.Rest.StoreManagementRestWrapper _adlsAccountMgmtClient;
        private MSAZURERM.ResourceManagementClient _RmClient;

        public readonly AnalyticsResourceCommands Analytics;
        public readonly StoreResourceCommands Store;

        public AzureClient(AdlClient.Authentication.AuthenticatedSession auth) :
            base(auth)
        {
            this.Analytics = new AnalyticsResourceCommands(auth);
            this.Store = new StoreResourceCommands(auth);

            this._RmClient = new MSAZURERM.ResourceManagementClient(auth.Credentials);
        }


        public IEnumerable<MSAZURERM.Models.Subscription> ListSubscriptions()
        {
            var sub_client = new MSAZURERM.SubscriptionClient(this.AuthenticatedSession.Credentials);

            var subs = sub_client.Subscriptions.List();
            foreach (var sub in subs)
            {
                yield return sub;
            }
        }

        // ----------
        public IEnumerable<MSAZURERM.Models.ResourceGroup> ListResourceGroups(string subid)
        {
            var rm_client = new MSAZURERM.ResourceManagementClient(this.AuthenticatedSession.Credentials);
            rm_client.SubscriptionId = subid;

            var rgs = rm_client.ResourceGroups.List();
            foreach (var rg in rgs)
            {
                yield return rg;
            }
        }

    }
}

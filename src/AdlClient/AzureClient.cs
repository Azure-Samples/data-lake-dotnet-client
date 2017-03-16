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

        public readonly Subscription Subscription;
        public readonly AnalyticsResourceCommands Analytics;
        public readonly StoreResourceCommands Store;

        public AzureClient(Subscription sub, AdlClient.Authentication.AuthenticatedSession auth) :
            base(auth)
        {
            this.Subscription = sub;
            this._adlaAccountMgmtClientWrapper = new AdlClient.Rest.AnalyticsAccountManagmentRestWrapper(sub, auth.Credentials);
            this._adlsAccountMgmtClient = new AdlClient.Rest.StoreManagementRestWrapper(sub, auth.Credentials);

            this.Analytics = new AnalyticsResourceCommands(sub, auth, _adlaAccountMgmtClientWrapper);
            this.Store = new StoreResourceCommands(sub,auth,this._adlsAccountMgmtClient);

            this._RmClient = new MSAZURERM.ResourceManagementClient(auth.Credentials);
            this._RmClient.SubscriptionId = sub.Id;
        }

        public IEnumerable<MSAZURERM.Models.ResourceGroup> ListResourceGroups()
        {
            var rgs = this._RmClient.ResourceGroups.List();
            foreach (var rg in rgs)
            {
                yield return rg;
            }
        }

    }
}

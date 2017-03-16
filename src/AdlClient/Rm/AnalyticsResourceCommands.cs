using System.Collections.Generic;
using AdlClient.Authentication;
using AdlClient.Rest;
using Microsoft.Azure.Management.DataLake.Analytics;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient
{
    public class AnalyticsResourceCommands
    {
        private readonly AnalyticsAccountManagmentRestWrapper _adlaAccountMgmtClientWrapper;

        private AuthenticatedSession _auth;

        public AnalyticsResourceCommands(AuthenticatedSession auth)
        {
            this._auth = auth;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccountsInSubscription(string sub)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = sub;
            return client.Account.List();
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccountsResourceGroup(string sub, string resource_group)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = sub;
            return client.Account.ListByResourceGroup(resource_group);
        }

        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(string sub, string resource_group, string account)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = sub;
            return client.Account.Get(resource_group, account);
        }


        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccount account)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = account.Subscription;
            return client.Account.Get(account.ResourceGroup, account.Name);
        }

        public bool AccountExists(string sub, string resource_group, string account)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = sub;
            return client.Account.Exists(resource_group, account);
        }

        public bool AccountExists(AnalyticsAccount account)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = account.Subscription;
            return client.Account.Exists(account.ResourceGroup, account.Name);
        }
    }
}
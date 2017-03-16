using System.Collections.Generic;
using AdlClient.Authentication;
using AdlClient.Rest;
using Microsoft.Azure.Management.DataLake.Analytics;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient
{
    public class AnalyticsResourceCommands
    {
        private AuthenticatedSession _auth;

        public AnalyticsResourceCommands(AuthenticatedSession auth)
        {
            this._auth = auth;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccountsInSubscription(string subid)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = subid;
            return client.Account.List();
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccountsResourceGroup(string subid, string rg)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = subid;
            return client.Account.ListByResourceGroup(rg);
        }

        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(string sub, string rg, string account)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = sub;
            return client.Account.Get(rg, account);
        }


        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccount account)
        {
            return this.GetAccount(account.SubscriptionId, account.ResourceGroup, account.Name);
        }

        public bool AccountExists(string subid, string rg, string account)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = subid;
            return client.Account.Exists(rg, account);
        }

        public bool AccountExists(AnalyticsAccount account)
        {
            return this.AccountExists(account.SubscriptionId, account.ResourceGroup, account.Name);
        }
    }
}
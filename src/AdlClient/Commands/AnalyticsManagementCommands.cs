using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Commands
{
    public class AnalyticsManagementCommands
    {
        private Authentication _auth;

        public AnalyticsManagementCommands(Authentication auth)
        {
            this._auth = auth;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccountsInSubscription(string subid)
        {
            var client = _get_acount_mgmt_client(subid);
            return client.Account.List();
        }

        private DataLakeAnalyticsAccountManagementClient _get_acount_mgmt_client(string subid)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(_auth.Credentials);
            client.SubscriptionId = subid;
            return client;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccountsResourceGroup(string subid, string rg)
        {
            var client = _get_acount_mgmt_client(subid);
            return client.Account.ListByResourceGroup(rg);
        }

        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(string subid, string rg, string account)
        {
            var client = _get_acount_mgmt_client(subid);
            return client.Account.Get(rg, account);
        }


        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccountRef account)
        {
            return this.GetAccount(account.SubscriptionId, account.ResourceGroup, account.Name);
        }

        public bool AccountExists(string subid, string rg, string account)
        {
            var client = _get_acount_mgmt_client(subid);
            return client.Account.Exists(rg, account);
        }

        public bool AccountExists(AnalyticsAccountRef account)
        {
            return this.AccountExists(account.SubscriptionId, account.ResourceGroup, account.Name);
        }

        public AdlClient.AnalyticsClient ConnectToAccount(string subid, string rg, string account)
        {
            var acct = new AdlClient.AnalyticsAccountRef(subid, rg, account);
            var adla = new AdlClient.AnalyticsClient(this._auth, acct);
            return adla;
        }
    }
}
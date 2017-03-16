using System.Collections.Generic;
using AdlClient.Authentication;
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


        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccount account)
        {
            return this.GetAccount(account.SubscriptionId, account.ResourceGroup, account.Name);
        }

        public bool AccountExists(string subid, string rg, string account)
        {
            var client = _get_acount_mgmt_client(subid);
            return client.Account.Exists(rg, account);
        }

        public bool AccountExists(AnalyticsAccount account)
        {
            return this.AccountExists(account.SubscriptionId, account.ResourceGroup, account.Name);
        }

        public AdlClient.AnalyticsClient ConnectToAccount(string subid, string rg, string account)
        {
            var acct = new AdlClient.AnalyticsAccount(subid, rg, account);
            var adla = new AdlClient.AnalyticsClient(acct, this._auth);
            return adla;
        }
    }
}
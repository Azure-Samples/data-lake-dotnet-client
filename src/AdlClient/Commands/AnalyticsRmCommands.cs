using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Commands
{
    public class AnalyticsRmCommands
    {
        internal readonly Authentication Authentication;

        internal AnalyticsRmCommands(Authentication auth)
        {
            this.Authentication = auth;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccountBasic> ListAccountsInSubscription(string subid)
        {
            var client = _get_acount_mgmt_client(subid);
            return client.Account.List();
        }

        private DataLakeAnalyticsAccountManagementClient _get_acount_mgmt_client(string subid)
        {
            var client = new MSADLA.DataLakeAnalyticsAccountManagementClient(Authentication.ARMCreds);
            client.SubscriptionId = subid;
            return client;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccountBasic> ListAccountsResourceGroup(string subid, string rg)
        {
            var client = _get_acount_mgmt_client(subid);
            return client.Account.ListByResourceGroup(rg);
        }

        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(string subid, string rg, string account)
        {
            var client = _get_acount_mgmt_client(subid);
            return client.Account.Get(rg, account);
        }


        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(AdlClient.Models.AnalyticsAccountRef account)
        {
            return this.GetAccount(account.SubscriptionId, account.ResourceGroup, account.Name);
        }

        public bool AccountExists(string subid, string rg, string account)
        {
            var client = _get_acount_mgmt_client(subid);
            return client.Account.Exists(rg, account);
        }

        public bool AccountExists(AdlClient.Models.AnalyticsAccountRef account)
        {
            return this.AccountExists(account.SubscriptionId, account.ResourceGroup, account.Name);
        }
    }
}
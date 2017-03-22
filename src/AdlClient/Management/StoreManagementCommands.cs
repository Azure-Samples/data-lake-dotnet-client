using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Store;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient
{
    public class StoreManagementCommands
    {
        private Authentication _auth;

        public StoreManagementCommands(Authentication authSession)
        {
            this._auth = authSession;
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccountsInSubscription(string subid)
        {
            var client = _get_account_mgmt_client(subid);
            return client.Account.List();
        }

        private DataLakeStoreAccountManagementClient _get_account_mgmt_client(string subid)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this._auth.Credentials);
            client.SubscriptionId = subid;
            return client;
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccountsInResourceGroup(string subid, string rg)
        {
            var client = _get_account_mgmt_client(subid);
            return client.Account.ListByResourceGroup(rg);
        }

        public MSADLS.Models.DataLakeStoreAccount GetAccount(string subid, string rg, string account)
        {
            var client = _get_account_mgmt_client(subid);
            return client.Account.Get(rg, account);
        }

        public MSADLS.Models.DataLakeStoreAccount GetAccount(StoreAccount account)
        {
            return this.GetAccount(account.SubscriptionId, account.ResourceGroup, account.Name);
        }


        public bool AccountExsists(string subid, string rg, string account)
        {
            var client = _get_account_mgmt_client(subid);
            return client.Account.Exists(rg, account);
        }

        public bool AccountExsists(StoreAccount account)
        {
            var client = _get_account_mgmt_client(account.SubscriptionId);
            return client.Account.Exists(account.ResourceGroup, account.Name);
        }
    }
}
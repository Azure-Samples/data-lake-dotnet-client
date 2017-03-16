using System.Collections.Generic;
using AdlClient.Authentication;
using Microsoft.Azure.Management.DataLake.Store;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient
{
    public class StoreResourceCommands
    {
        private AuthenticatedSession auth;

        public StoreResourceCommands(AuthenticatedSession authSession)
        {
            this.auth = authSession;
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccountsInSubscription(string subid)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = subid;
            return client.Account.List();
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccountsInResourceGroup(string subid, string rg)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = subid;
            return client.Account.ListByResourceGroup(rg);
        }

        public MSADLS.Models.DataLakeStoreAccount GetAccount(string subid, string rg, string account)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = subid;
            return client.Account.Get(rg, account);
        }

        public MSADLS.Models.DataLakeStoreAccount GetAccount(StoreAccount account)
        {
            return this.GetAccount(account.SubscriptionId, account.ResourceGroup, account.Name);
        }


        public bool AccountExsists(string subid, string rg, string account)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = subid;
            return client.Account.Exists(rg, account);
        }

        public bool AccountExsists(StoreAccount account)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = account.SubscriptionId;
            return client.Account.Exists(account.ResourceGroup, account.Name);
        }
    }
}
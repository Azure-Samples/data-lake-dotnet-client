using System.Collections.Generic;
using AdlClient.Authentication;
using AdlClient.Rest;
using Microsoft.Azure.Management.DataLake.Store;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;
using MSAZURERM = Microsoft.Azure.Management.ResourceManager;

namespace AdlClient
{
    public class StoreResourceCommands
    {
        private AuthenticatedSession auth;

        public StoreResourceCommands(AuthenticatedSession authSession)
        {
            this.auth = authSession;
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccountsInSubscription(string sub)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = sub;
            return client.Account.List();
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccountsInResourceGroup(string sub, string resource_group)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = sub;
            return client.Account.ListByResourceGroup(resource_group);
        }

        public MSADLS.Models.DataLakeStoreAccount GetAccount(string sub, string resource_group, string account)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = sub;
            return client.Account.Get(resource_group, account);
        }

        public MSADLS.Models.DataLakeStoreAccount GetAccount(StoreAccount account)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = account.Subscription;
            return client.Account.Get(account.ResourceGroup, account.Name);
        }


        public bool AccountExsists(string sub, string resource_group, string account)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = sub;
            return client.Account.Exists(resource_group, account);
        }

        public bool AccountExsists(StoreAccount account)
        {
            var client = new MSADLS.DataLakeStoreAccountManagementClient(this.auth.Credentials);
            client.SubscriptionId = account.Subscription;
            return client.Account.Exists(account.ResourceGroup, account.Name);
        }

    }
}
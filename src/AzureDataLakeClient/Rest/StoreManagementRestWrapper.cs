using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient.Rest
{
    public class StoreManagementRestWrapper
    {
        public readonly Microsoft.Azure.Management.DataLake.Store.DataLakeStoreAccountManagementClient RestClient;

        public StoreManagementRestWrapper(Subscription sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.RestClient = new DataLakeStoreAccountManagementClient(creds);
            this.RestClient.SubscriptionId = sub.Id;
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Store.Models.DataLakeStoreAccount> ListAccounts()
        {
            var page = this.RestClient.Account.List();
            foreach (var acc in RestUtil.EnumItemsInPages(page,
                p => this.RestClient.Account.ListNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Store.Models.DataLakeStoreAccount> ListAccountsByResourceGroup(ResourceGroup resource_group)
        {
            var page = this.RestClient.Account.ListByResourceGroup(resource_group.Name);

            foreach (var acc in RestUtil.EnumItemsInPages(page,
                p => this.RestClient.Account.ListByResourceGroupNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public Microsoft.Azure.Management.DataLake.Store.Models.DataLakeStoreAccount GetAccount(StoreAccount account)
        {
            return this.RestClient.Account.Get(account.ResourceGroup.Name, account.Name);
        }

        public void Update(StoreAccount account, Microsoft.Azure.Management.DataLake.Store.Models.DataLakeStoreAccountUpdateParameters parameters)
        {
            this.RestClient.Account.Update(account.ResourceGroup.Name, account.Name, parameters);
        }

        public void Delete(StoreAccount account)
        {
            this.RestClient.Account.Delete(account.ResourceGroup.Name, account.Name);
        }

        public bool Exists(StoreAccount account)
        {
            return this.RestClient.Account.Exists(account.ResourceGroup.Name, account.Name);
        }

    }
}
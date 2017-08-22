using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Store;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient.Rest
{
    public class StoreManagementRestWrapper
    {
        public readonly MSADLS.DataLakeStoreAccountManagementClient RestClient;

        public StoreManagementRestWrapper(string sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.RestClient = new DataLakeStoreAccountManagementClient(creds);
            this.RestClient.SubscriptionId = sub;
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccounts()
        {
            var pageiter = new PagedIterator<MSADLS.Models.DataLakeStoreAccount>();
            pageiter.GetFirstPage = () => this.RestClient.Account.List();
            pageiter.GetNextPage = p => this.RestClient.Account.ListNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccountsByResourceGroup(string resource_group)
        {
            var pageiter = new PagedIterator<MSADLS.Models.DataLakeStoreAccount>();
            pageiter.GetFirstPage = () => this.RestClient.Account.ListByResourceGroup(resource_group);
            pageiter.GetNextPage = p => this.RestClient.Account.ListByResourceGroupNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;

        }

        public MSADLS.Models.DataLakeStoreAccount GetAccount(AdlClient.Models.StoreAccountRef account)
        {
            return this.RestClient.Account.Get(account.ResourceGroup, account.Name);
        }

        public void Update(AdlClient.Models.StoreAccountRef account, MSADLS.Models.DataLakeStoreAccountUpdateParameters parameters)
        {
            this.RestClient.Account.Update(account.ResourceGroup, account.Name, parameters);
        }

        public void Delete(AdlClient.Models.StoreAccountRef account)
        {
            this.RestClient.Account.Delete(account.ResourceGroup, account.Name);
        }

        public bool Exists(AdlClient.Models.StoreAccountRef account)
        {
            return this.RestClient.Account.Exists(account.ResourceGroup, account.Name);
        }

    }
}
using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Rest.Azure;
// have to have this using clause to get the extension methods
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Rest
{
    public class AnalyticsAccountManagmentRestWrapper
    {
        public MSADLA.DataLakeAnalyticsAccountManagementClient RestClient;

        public AnalyticsAccountManagmentRestWrapper(string sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.RestClient = new MSADLA.DataLakeAnalyticsAccountManagementClient(creds);
            this.RestClient.SubscriptionId = sub;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var pageiter = new PagedIterator<MSADLA.Models.DataLakeAnalyticsAccount>();
            pageiter.GetFirstPage = () => this.RestClient.Account.List();
            pageiter.GetNextPage = p => this.RestClient.Account.ListNext(p.NextPageLink);

            int top = 0;

            var accounts = RestUtil.EnumItems(pageiter, top);

            return accounts;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccounts(string rg)
        {
            var pageiter = new PagedIterator<MSADLA.Models.DataLakeAnalyticsAccount>();
            pageiter.GetFirstPage = () => this.RestClient.Account.ListByResourceGroup(rg);
            pageiter.GetNextPage = p => this.RestClient.Account.ListByResourceGroupNext(p.NextPageLink);

            int top = 0;

            var accounts = RestUtil.EnumItems(pageiter, top);

            return accounts;
        }

        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(AdlClient.Models.AnalyticsAccountRef account)
        {
            var adls_account = this.RestClient.Account.Get(account.ResourceGroup, account.Name);
            return adls_account;
        }

        public bool ExistsAccount(AdlClient.Models.AnalyticsAccountRef account)
        {
            return this.RestClient.Account.Exists(account.ResourceGroup, account.Name);
        }

        public void UpdateAccount(AdlClient.Models.AnalyticsAccountRef account, MSADLA.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this.RestClient.Account.Update(account.ResourceGroup, account.Name, parameters);
        }

        public void AddStorageAccount(AdlClient.Models.AnalyticsAccountRef account, string storage_account, MSADLA.Models.AddStorageAccountParameters parameters)
        {
            this.RestClient.StorageAccounts.Add(account.ResourceGroup, account.Name, storage_account, parameters);
        }

        public void AddDataLakeStoreAccount(AdlClient.Models.AnalyticsAccountRef account, string storage_account, MSADLA.Models.AddDataLakeStoreParameters parameters)
        {
            this.RestClient.DataLakeStoreAccounts.Add(account.ResourceGroup, account.Name, storage_account, parameters);
        }

        public IEnumerable<MSADLA.Models.DataLakeStoreAccountInfo> ListStoreAccounts(AdlClient.Models.AnalyticsAccountRef account)
        {
            var pageiter = new PagedIterator<MSADLA.Models.DataLakeStoreAccountInfo>();
            pageiter.GetFirstPage = () => this.RestClient.DataLakeStoreAccounts.ListByAccount(account.ResourceGroup, account.Name);
            pageiter.GetNextPage = p => this.RestClient.DataLakeStoreAccounts.ListByAccountNext(p.NextPageLink);

            int top = 0;
            var accounts = RestUtil.EnumItems(pageiter, top);

            return accounts;
        }

        public IEnumerable<MSADLA.Models.StorageAccountInfo> ListStorageAccounts(AdlClient.Models.AnalyticsAccountRef account)
        {

            var pageiter = new PagedIterator<MSADLA.Models.StorageAccountInfo>();
            pageiter.GetFirstPage = () => this.RestClient.StorageAccounts.ListByAccount(account.ResourceGroup, account.Name);
            pageiter.GetNextPage = p => this.RestClient.StorageAccounts.ListByAccountNext(p.NextPageLink);

            int top = 0;
            var accounts = RestUtil.EnumItems(pageiter, top);

            return accounts;
        }

        public IEnumerable<MSADLA.Models.StorageContainer> ListStorageContainers(AdlClient.Models.AnalyticsAccountRef account, string storage_account)
        {
            var pageiter = new PagedIterator<MSADLA.Models.StorageContainer>();
            pageiter.GetFirstPage = () => this.RestClient.StorageAccounts.ListStorageContainers(account.Name, account.Name, storage_account);
            pageiter.GetNextPage = p => this.RestClient.StorageAccounts.ListStorageContainersNext(p.NextPageLink);

            int top = 0;
            var items = RestUtil.EnumItems(pageiter, top);
            return items;
        }

        public void DeleteStorageAccount(AdlClient.Models.AnalyticsAccountRef account, string storage_account)
        {
            this.RestClient.StorageAccounts.Delete(account.ResourceGroup, account.Name, storage_account);
        }

        public void DeleteDataLakeStoreAccount(AdlClient.Models.AnalyticsAccountRef account, string storage_account)
        {
            this.RestClient.DataLakeStoreAccounts.Delete(account.ResourceGroup, account.Name, storage_account);
        }

        public IEnumerable<MSADLA.Models.SasTokenInfo> ListSasTokens(AdlClient.Models.AnalyticsAccountRef account, string storage_account, string container)
        {
            var pageiter = new PagedIterator<MSADLA.Models.SasTokenInfo>();
            pageiter.GetFirstPage = () => this.RestClient.StorageAccounts.ListSasTokens(account.ResourceGroup, account.Name, storage_account, container);
            pageiter.GetNextPage = p => this.RestClient.StorageAccounts.ListSasTokensNext(p.NextPageLink);

            int top = 0;
            var items = RestUtil.EnumItems(pageiter, top);
            return items;
        }
    }
}

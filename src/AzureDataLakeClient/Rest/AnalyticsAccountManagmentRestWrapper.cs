using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics; // have to have this using clause to get the extension methods
using MSADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Rest
{
    public class AnalyticsAccountManagmentRestWrapper
    {
        public MSADL.Analytics.DataLakeAnalyticsAccountManagementClient RestClient;

        public AnalyticsAccountManagmentRestWrapper(Subscription sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.RestClient = new MSADL.Analytics.DataLakeAnalyticsAccountManagementClient(creds);
            this.RestClient.SubscriptionId = sub.Id;
        }

        public IEnumerable<MSADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var initial_page = this.RestClient.Account.List();
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.Account.ListNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<MSADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts(ResourceGroup rg)
        {
            var initial_page = this.RestClient.Account.ListByResourceGroup(rg.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.Account.ListByResourceGroupNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public MSADL.Analytics.Models.DataLakeAnalyticsAccount GetAccount(ResourceGroup rg, AnalyticsAccount account)
        {
            var adls_account = this.RestClient.Account.Get(rg.Name, account.Name);
            return adls_account;
        }

        public bool ExistsAccount(ResourceGroup rg, AnalyticsAccount account_name)
        {
            return this.RestClient.Account.Exists(rg.Name, account_name.Name);
        }

        public void UpdateAccount(ResourceGroup rg, AnalyticsAccount account, MSADL.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this.RestClient.Account.Update(rg.Name, account.Name, parameters);
        }

        public void AddStorageAccount(ResourceGroup rg, AnalyticsAccount account, string storage_account, MSADL.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this.RestClient.StorageAccounts.Add(rg.Name, account.Name, storage_account, parameters);
        }

        public void AddDataLakeStoreAccount(ResourceGroup rg, AnalyticsAccount account, string storage_account, MSADL.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this.RestClient.DataLakeStoreAccounts.Add(rg.Name, account.Name, storage_account, parameters);
        }

        public IEnumerable<MSADL.Analytics.Models.DataLakeStoreAccountInfo> ListStoreAccounts(ResourceGroup rg, AnalyticsAccount account)
        {
            var initial_page = this.RestClient.DataLakeStoreAccounts.ListByAccount(rg.Name, account.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.DataLakeStoreAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<MSADL.Analytics.Models.StorageAccountInfo> ListStorageAccounts(ResourceGroup rg, AnalyticsAccount account)
        {
            var initial_page = this.RestClient.StorageAccounts.ListByAccount(rg.Name, account.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.StorageAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<MSADL.Analytics.Models.StorageContainer> ListStorageContainers(ResourceGroup rg, AnalyticsAccount account, string storage_account)
        {
            var initial_page = this.RestClient.StorageAccounts.ListStorageContainers(rg.Name, account.Name, storage_account);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.StorageAccounts.ListStorageContainersNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public void DeleteStorageAccount(ResourceGroup resource_group, AnalyticsAccount account, string storage_account)
        {
            this.RestClient.StorageAccounts.Delete(resource_group.Name, account.Name, storage_account);
        }

        public void DeleteDataLakeStoreAccount(ResourceGroup resource_group, AnalyticsAccount account, string storage_account)
        {
            this.RestClient.DataLakeStoreAccounts.Delete(resource_group.Name, account.Name, storage_account);
        }

        public IEnumerable<MSADL.Analytics.Models.SasTokenInfo> ListSasTokens(ResourceGroup resource_group, AnalyticsAccount account, string storage_account, string container)
        {
            var initial_page = this.RestClient.StorageAccounts.ListSasTokens(resource_group.Name, account.Name, storage_account, container);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.StorageAccounts.ListSasTokensNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }
    }
}

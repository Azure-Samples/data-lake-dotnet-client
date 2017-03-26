using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics; // have to have this using clause to get the extension methods
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
            var initial_page = this.RestClient.Account.List();
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.Account.ListNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccounts(string rg)
        {
            var initial_page = this.RestClient.Account.ListByResourceGroup(rg);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.Account.ListByResourceGroupNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccountRef account)
        {
            var adls_account = this.RestClient.Account.Get(account.ResourceGroup, account.Name);
            return adls_account;
        }

        public bool ExistsAccount(AnalyticsAccountRef account)
        {
            return this.RestClient.Account.Exists(account.ResourceGroup, account.Name);
        }

        public void UpdateAccount(AnalyticsAccountRef account, MSADLA.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this.RestClient.Account.Update(account.ResourceGroup, account.Name, parameters);
        }

        public void AddStorageAccount(AnalyticsAccountRef account, string storage_account, MSADLA.Models.AddStorageAccountParameters parameters)
        {
            this.RestClient.StorageAccounts.Add(account.ResourceGroup, account.Name, storage_account, parameters);
        }

        public void AddDataLakeStoreAccount(AnalyticsAccountRef account, string storage_account, MSADLA.Models.AddDataLakeStoreParameters parameters)
        {
            this.RestClient.DataLakeStoreAccounts.Add(account.ResourceGroup, account.Name, storage_account, parameters);
        }

        public IEnumerable<MSADLA.Models.DataLakeStoreAccountInfo> ListStoreAccounts(AnalyticsAccountRef account)
        {
            var initial_page = this.RestClient.DataLakeStoreAccounts.ListByAccount(account.ResourceGroup, account.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.DataLakeStoreAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<MSADLA.Models.StorageAccountInfo> ListStorageAccounts(AnalyticsAccountRef account)
        {
            var initial_page = this.RestClient.StorageAccounts.ListByAccount(account.ResourceGroup, account.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.StorageAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<MSADLA.Models.StorageContainer> ListStorageContainers(AnalyticsAccountRef account, string storage_account)
        {
            var initial_page = this.RestClient.StorageAccounts.ListStorageContainers(account.Name, account.Name, storage_account);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.StorageAccounts.ListStorageContainersNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public void DeleteStorageAccount(AnalyticsAccountRef account, string storage_account)
        {
            this.RestClient.StorageAccounts.Delete(account.ResourceGroup, account.Name, storage_account);
        }

        public void DeleteDataLakeStoreAccount(AnalyticsAccountRef account, string storage_account)
        {
            this.RestClient.DataLakeStoreAccounts.Delete(account.ResourceGroup, account.Name, storage_account);
        }

        public IEnumerable<MSADLA.Models.SasTokenInfo> ListSasTokens(AnalyticsAccountRef account, string storage_account, string container)
        {
            var initial_page = this.RestClient.StorageAccounts.ListSasTokens(account.ResourceGroup, account.Name, storage_account, container);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this.RestClient.StorageAccounts.ListSasTokensNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }
    }
}

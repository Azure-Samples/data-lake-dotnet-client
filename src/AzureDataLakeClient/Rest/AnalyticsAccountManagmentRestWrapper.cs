using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics; // have to have this using clause to get the extension methods
using MSADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Rest
{
    public class AnalyticsAccountManagmentRestWrapper
    {
        private MSADL.Analytics.DataLakeAnalyticsAccountManagementClient _adla_acctmgmt_client;

        public AnalyticsAccountManagmentRestWrapper(Subscription sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._adla_acctmgmt_client = new MSADL.Analytics.DataLakeAnalyticsAccountManagementClient(creds);
            this._adla_acctmgmt_client.SubscriptionId = sub.Id;
        }

        public IEnumerable<MSADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var initial_page = this._adla_acctmgmt_client.Account.List();
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.Account.ListNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<MSADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts(ResourceGroup rg)
        {
            var initial_page = this._adla_acctmgmt_client.Account.ListByResourceGroup(rg.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.Account.ListByResourceGroupNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public MSADL.Analytics.Models.DataLakeAnalyticsAccount GetAccount(ResourceGroup rg, AnalyticsAccount account)
        {
            var adls_account = this._adla_acctmgmt_client.Account.Get(rg.Name, account.Name);
            return adls_account;
        }

        public bool ExistsAccount(ResourceGroup rg, AnalyticsAccount account_name)
        {
            return this._adla_acctmgmt_client.Account.Exists(rg.Name, account_name.Name);
        }

        public void UpdateAccount(ResourceGroup rg, AnalyticsAccount account, MSADL.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this._adla_acctmgmt_client.Account.Update(rg.Name, account.Name, parameters);
        }

        public void AddStorageAccount(ResourceGroup rg, AnalyticsAccount account, string storage_account, MSADL.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._adla_acctmgmt_client.StorageAccounts.Add(rg.Name, account.Name, storage_account, parameters);
        }

        public void AddDataLakeStoreAccount(ResourceGroup rg, AnalyticsAccount account, string storage_account, MSADL.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._adla_acctmgmt_client.DataLakeStoreAccounts.Add(rg.Name, account.Name, storage_account, parameters);
        }

        public IEnumerable<MSADL.Analytics.Models.DataLakeStoreAccountInfo> ListStoreAccounts(ResourceGroup rg, AnalyticsAccount account)
        {
            var initial_page = this._adla_acctmgmt_client.DataLakeStoreAccounts.ListByAccount(rg.Name, account.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.DataLakeStoreAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<MSADL.Analytics.Models.StorageAccountInfo> ListStorageAccounts(ResourceGroup rg, AnalyticsAccount account)
        {
            var initial_page = this._adla_acctmgmt_client.StorageAccounts.ListByAccount(rg.Name, account.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.StorageAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<MSADL.Analytics.Models.StorageContainer> ListStorageContainers(ResourceGroup rg, AnalyticsAccount account, string storage_account)
        {
            var initial_page = this._adla_acctmgmt_client.StorageAccounts.ListStorageContainers(rg.Name, account.Name, storage_account);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.StorageAccounts.ListStorageContainersNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public void DeleteStorageAccount(ResourceGroup resource_group, AnalyticsAccount account, string storage_account)
        {
            this._adla_acctmgmt_client.StorageAccounts.Delete(resource_group.Name, account.Name, storage_account);
        }

        public void DeleteDataLakeStoreAccount(ResourceGroup resource_group, AnalyticsAccount account, string storage_account)
        {
            this._adla_acctmgmt_client.DataLakeStoreAccounts.Delete(resource_group.Name, account.Name, storage_account);
        }

        public IEnumerable<MSADL.Analytics.Models.SasTokenInfo> ListSasTokens(ResourceGroup resource_group, AnalyticsAccount account, string storage_account, string container)
        {
            var initial_page = this._adla_acctmgmt_client.StorageAccounts.ListSasTokens(resource_group.Name, account.Name, storage_account, container);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.StorageAccounts.ListSasTokensNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }
    }
}

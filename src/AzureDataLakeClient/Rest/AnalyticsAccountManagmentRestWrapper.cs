using System.Collections.Generic;
using AzureDataLakeClient.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Rest
{
    public class AnalyticsAccountManagmentRestWrapper
    {
        private ADL.Analytics.DataLakeAnalyticsAccountManagementClient _adla_acctmgmt_client;

        public AnalyticsAccountManagmentRestWrapper(AzureDataLakeClient.Rm.Subscription sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._adla_acctmgmt_client = new ADL.Analytics.DataLakeAnalyticsAccountManagementClient(creds);
            this._adla_acctmgmt_client.SubscriptionId = sub.Id;
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var initial_page = this._adla_acctmgmt_client.Account.List();
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.Account.ListNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts(AzureDataLakeClient.Rm.ResourceGroup rg)
        {
            var initial_page = this._adla_acctmgmt_client.Account.ListByResourceGroup(rg.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.Account.ListByResourceGroupNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAccount(AzureDataLakeClient.Rm.ResourceGroup rg, AnalyticsAccountUri account)
        {
            var adls_account = this._adla_acctmgmt_client.Account.Get(rg.Name, account.Name);
            return adls_account;
        }

        public bool ExistsAccount(AzureDataLakeClient.Rm.ResourceGroup rg, AnalyticsAccountUri account_name)
        {
            return this._adla_acctmgmt_client.Account.Exists(rg.Name, account_name.Name);
        }

        public void UpdateAccount(AzureDataLakeClient.Rm.ResourceGroup rg, AnalyticsAccountUri account, ADL.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this._adla_acctmgmt_client.Account.Update(rg.Name, account.Name, parameters);
        }

        public void AddStorageAccount(AzureDataLakeClient.Rm.ResourceGroup rg, AnalyticsAccountUri account, string storage_account, ADL.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._adla_acctmgmt_client.StorageAccounts.Add(rg.Name, account.Name, storage_account, parameters);
        }

        public void AddDataLakeStoreAccount(AzureDataLakeClient.Rm.ResourceGroup rg, AnalyticsAccountUri account, string storage_account, ADL.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._adla_acctmgmt_client.DataLakeStoreAccounts.Add(rg.Name, account.Name, storage_account, parameters);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeStoreAccountInfo> ListStoreAccounts(AzureDataLakeClient.Rm.ResourceGroup rg, AnalyticsAccountUri account)
        {
            var initial_page = this._adla_acctmgmt_client.DataLakeStoreAccounts.ListByAccount(rg.Name, account.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.DataLakeStoreAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.StorageAccountInfo> ListStorageAccounts(AzureDataLakeClient.Rm.ResourceGroup rg, AnalyticsAccountUri account)
        {
            var initial_page = this._adla_acctmgmt_client.StorageAccounts.ListByAccount(rg.Name, account.Name);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.StorageAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.StorageContainer> ListStorageContainers(AzureDataLakeClient.Rm.ResourceGroup rg, AnalyticsAccountUri account, string storage_account)
        {
            var initial_page = this._adla_acctmgmt_client.StorageAccounts.ListStorageContainers(rg.Name, account.Name, storage_account);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.StorageAccounts.ListStorageContainersNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public void DeleteStorageAccount(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, string storage_account)
        {
            this._adla_acctmgmt_client.StorageAccounts.Delete(resource_group.Name, account.Name, storage_account);
        }

        public void DeleteDataLakeStoreAccount(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, string storage_account)
        {
            this._adla_acctmgmt_client.DataLakeStoreAccounts.Delete(resource_group.Name, account.Name, storage_account);
        }

        public IEnumerable<ADL.Analytics.Models.SasTokenInfo> ListSasTokens(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, string storage_account, string container)
        {
            var initial_page = this._adla_acctmgmt_client.StorageAccounts.ListSasTokens(resource_group.Name, account.Name, storage_account, container);
            foreach (var acc in RestUtil.EnumItemsInPages(initial_page, p => this._adla_acctmgmt_client.StorageAccounts.ListSasTokensNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

    }
}

using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsRmRestClient
    {
        private ADL.Analytics.DataLakeAnalyticsAccountManagementClient _rest_client;

        public AnalyticsRmRestClient(AzureDataLakeClient.Rm.Subscription sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._rest_client = new ADL.Analytics.DataLakeAnalyticsAccountManagementClient(creds);
            this._rest_client.SubscriptionId = sub.ID;
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var initial_page = this._rest_client.Account.List();
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.Account.ListNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccountsByResourceGroup(AzureDataLakeClient.Rm.ResourceGroup resource_group)
        {
            var initial_page = this._rest_client.Account.ListByResourceGroup(resource_group.Name);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.Account.ListByResourceGroupNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAccount(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account)
        {
            var adls_account = this._rest_client.Account.Get(resource_group.Name, account.Name);
            return adls_account;
        }

        public bool ExistsAccount(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account_name)
        {
            return this._rest_client.Account.Exists(resource_group.Name, account_name.Name);
        }

        public void UpdateAccount(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, ADL.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this._rest_client.Account.Update(resource_group.Name, account.Name, parameters);
        }

        public void AddStorageAccount(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, string storage_account, ADL.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._rest_client.StorageAccounts.Add(resource_group.Name, account.Name, storage_account, parameters);
        }

        public void AddDataLakeStoreAccount(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, string storage_account, ADL.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._rest_client.DataLakeStoreAccounts.Add(resource_group.Name, account.Name, storage_account, parameters);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeStoreAccountInfo> ListStoreAccounts(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account)
        {
            var initial_page = this._rest_client.DataLakeStoreAccounts.ListByAccount(resource_group.Name, account.Name);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.DataLakeStoreAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.StorageAccountInfo> ListStorageAccounts(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account)
        {
            var initial_page = this._rest_client.StorageAccounts.ListByAccount(resource_group.Name, account.Name);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.StorageAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.StorageContainer> ListStorageContainers(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, string storage_account)
        {
            var initial_page = this._rest_client.StorageAccounts.ListStorageContainers(resource_group.Name, account.Name, storage_account);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.StorageAccounts.ListStorageContainersNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public void DeleteStorageAccount(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, string storage_account)
        {
            this._rest_client.StorageAccounts.Delete(resource_group.Name, account.Name, storage_account);
        }

        public void DeleteDataLakeStoreAccount(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, string storage_account)
        {
            this._rest_client.DataLakeStoreAccounts.Delete(resource_group.Name, account.Name, storage_account);
        }

        public IEnumerable<ADL.Analytics.Models.SasTokenInfo> ListSasTokens(AzureDataLakeClient.Rm.ResourceGroup resource_group, AnalyticsAccountUri account, string storage_account, string container)
        {
            var initial_page = this._rest_client.StorageAccounts.ListSasTokens(resource_group.Name, account.Name, storage_account, container);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.StorageAccounts.ListSasTokensNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

    }
}

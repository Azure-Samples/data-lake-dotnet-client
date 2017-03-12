using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsRmClient: ClientBase
    {
        private ADL.Analytics.DataLakeAnalyticsAccountManagementClient _rest_client;
        private AzureDataLakeClient.Rm.Subscription Sub;

        public AnalyticsRmClient(AzureDataLakeClient.Rm.Subscription sub, AuthenticatedSession authSession) :
            base(authSession)
        {

            this.Sub = sub;
            this._rest_client = new ADL.Analytics.DataLakeAnalyticsAccountManagementClient(this.AuthenticatedSession.Credentials);
            this._rest_client.SubscriptionId = sub.ID;
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var initial_page = this._rest_client.Account.List();
            foreach (var acc in  RESTUtil.EnumItemsInPages(initial_page,p => this._rest_client.Account.ListNext(p.NextPageLink)))
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

        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccount account)
        {
            var adls_account = this._rest_client.Account.Get(account.ResourceGroup.Name, account.Name);
            return adls_account;
        }

        public bool ExistsAccount(AnalyticsAccountRmRef account)
        {
            return this._rest_client.Account.Exists(account.ResourceGroup.Name, account.Name);
        }

        public void UpdateAccount(AnalyticsAccountRmRef account, ADL.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this._rest_client.Account.Update(account.ResourceGroup.Name, account.Name, parameters);
        }

        public void AddStorageAccount(AnalyticsAccountRmRef account, string storage_account, ADL.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._rest_client.StorageAccounts.Add(account.ResourceGroup.Name, account.Name, storage_account, parameters);
        }

        public void AddDataLakeStoreAccount(AnalyticsAccountRmRef account, string storage_account, ADL.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._rest_client.DataLakeStoreAccounts.Add(account.ResourceGroup.Name, account.Name, storage_account, parameters);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeStoreAccountInfo> ListStoreAccounts(AnalyticsAccount account)
        {
            var initial_page = this._rest_client.DataLakeStoreAccounts.ListByAccount(account.ResourceGroup.Name, account.Name);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.DataLakeStoreAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.StorageAccountInfo> ListStorageAccounts(AnalyticsAccountRmRef account)
        {
            var initial_page = this._rest_client.StorageAccounts.ListByAccount(account.ResourceGroup.Name, account.Name);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.StorageAccounts.ListByAccountNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.StorageContainer> ListStorageContainers(AnalyticsAccountRmRef account, string storage_account)
        {
            var initial_page = this._rest_client.StorageAccounts.ListStorageContainers(account.ResourceGroup.Name, account.Name, storage_account);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.StorageAccounts.ListStorageContainersNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public void DeleteStorageAccount(AnalyticsAccountRmRef account, string storage_account)
        {
            this._rest_client.StorageAccounts.Delete(account.ResourceGroup.Name, account.Name, storage_account);
        }

        public void DeleteDataLakeStoreAccount(AnalyticsAccountRmRef account, string storage_account)
        {
            this._rest_client.DataLakeStoreAccounts.Delete(account.ResourceGroup.Name, account.Name, storage_account);
        }

        public IEnumerable<ADL.Analytics.Models.SasTokenInfo> ListSasTokens(AnalyticsAccountRmRef account, string storage_account, string container)
        {
            var initial_page = this._rest_client.StorageAccounts.ListSasTokens(account.ResourceGroup.Name, account.Name, storage_account, container);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._rest_client.StorageAccounts.ListSasTokensNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }
    }
}

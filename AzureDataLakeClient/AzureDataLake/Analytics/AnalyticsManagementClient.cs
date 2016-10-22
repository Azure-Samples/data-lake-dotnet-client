using System.Collections.Generic;
using System.Linq;
using AzureDataLakeClient.Authentication;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsManagementClient: ClientBase
    {
        private ADL.Analytics.DataLakeAnalyticsAccountManagementClient _adla_mgmt_rest_client;
        private Subscription Sub;

        public AnalyticsManagementClient(Subscription sub, AuthenticatedSession authSession) :
            base(authSession)
        {

            this.Sub = sub;
            this._adla_mgmt_rest_client = new ADL.Analytics.DataLakeAnalyticsAccountManagementClient(this.AuthenticatedSession.Credentials);
            this._adla_mgmt_rest_client.SubscriptionId = sub.ID;
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var initial_page = this._adla_mgmt_rest_client.Account.List();
            foreach (var acc in  RESTUtil.EnumItemsInPages(initial_page,p => this._adla_mgmt_rest_client.Account.ListNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccountsByResourceGroup(string resource_group)
        {
            var initial_page = this._adla_mgmt_rest_client.Account.ListByResourceGroup(resource_group);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._adla_mgmt_rest_client.Account.ListByResourceGroupNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAccount(string resource_group, string account_name)
        {
            var adls_account = this._adla_mgmt_rest_client.Account.Get(resource_group, account_name);
            return adls_account;
        }

        public bool ExistsAccount(string resource_group, string account_name)
        {
            return this._adla_mgmt_rest_client.Account.Exists(resource_group, account_name);
        }

        public void UpdateAccount(string resource_group, string account_name, ADL.Analytics.Models.DataLakeAnalyticsAccount parameters)
        {
            this._adla_mgmt_rest_client.Account.Update(resource_group, account_name, parameters);
        }

        public void AddStorageAccount(string resource_group, string name, string storage_account, ADL.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._adla_mgmt_rest_client.Account.AddStorageAccount(resource_group, name, storage_account, parameters);
        }

        public void AddDataLakeStoreAccount(string resource_group, string name, string storage_account, ADL.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._adla_mgmt_rest_client.Account.AddDataLakeStoreAccount(resource_group, name, storage_account, parameters);
        }
        
        public IEnumerable<ADL.Analytics.Models.StorageAccountInfo> ListStorageAccounts(string resource_group, string account)
        {
            var initial_page = this._adla_mgmt_rest_client.Account.ListStorageAccounts(resource_group, account);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._adla_mgmt_rest_client.Account.ListStorageAccountsNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.BlobContainer> ListStorageContainers(string resource_group, string account, string storage_account)
        {
            var initial_page = this._adla_mgmt_rest_client.Account.ListStorageContainers(resource_group, account, storage_account);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._adla_mgmt_rest_client.Account.ListStorageContainersNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public void DeleteStorageAccount(string resource_group, string account, string storage_account)
        {
            this._adla_mgmt_rest_client.Account.DeleteStorageAccount(resource_group, account, storage_account);
        }

        public void DeleteDataLakeStoreAccount(string resource_group, string account, string storage_account)
        {
            this._adla_mgmt_rest_client.Account.DeleteDataLakeStoreAccount(resource_group, account, storage_account);
        }

        public IEnumerable<ADL.Analytics.Models.SasTokenInfo> ListSasTokens(string resource_group, string account_name, string storage_account, string container)
        {
            var initial_page = this._adla_mgmt_rest_client.Account.ListSasTokens(resource_group, account_name, storage_account, container);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._adla_mgmt_rest_client.Account.ListSasTokensNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }
    }
}

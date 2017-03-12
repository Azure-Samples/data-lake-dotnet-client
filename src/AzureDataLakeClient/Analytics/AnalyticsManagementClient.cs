using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{

    public class AnalyticsRmClient: ClientBase
    {
        private AnalyticsRmRestClient _rest_client;
        private AzureDataLakeClient.Rm.Subscription Sub;

        public AnalyticsRmClient(AzureDataLakeClient.Rm.Subscription sub, AuthenticatedSession authSession) :
            base(authSession)
        {

            this.Sub = sub;
            this._rest_client = new AnalyticsRmRestClient(sub, authSession.Credentials);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            return this._rest_client.ListAccounts();
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccountsByResourceGroup(AzureDataLakeClient.Rm.ResourceGroup resource_group)
        {
            return this._rest_client.ListAccountsByResourceGroup(resource_group);
        }

        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccount account)
        {
            return this._rest_client.GetAccount(account.ResourceGroup, account.Name);
        }

        public bool ExistsAccount(AnalyticsAccount account)
        {
            return this._rest_client.ExistsAccount(account.ResourceGroup, account.Name);
        }

        public void UpdateAccount(AnalyticsAccount account, ADL.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this._rest_client.UpdateAccount(account.ResourceGroup, account.Name, parameters);
        }

   
        public void AddStorageAccount(AnalyticsAccount account, string storage_account, ADL.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._rest_client.AddStorageAccount(account.ResourceGroup, account.Name, storage_account, parameters);
        }

        public void AddDataLakeStoreAccount(AnalyticsAccount account, string storage_account, ADL.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._rest_client.AddDataLakeStoreAccount(account.ResourceGroup, account.Name, storage_account, parameters);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeStoreAccountInfo> ListStoreAccounts(AnalyticsAccount account)
        {
            return this._rest_client.ListStoreAccounts(account.ResourceGroup, account.Name);
        }

        public IEnumerable<ADL.Analytics.Models.StorageAccountInfo> ListStorageAccounts(AnalyticsAccount account)
        {
            return this._rest_client.ListStorageAccounts(account.ResourceGroup, account.Name);
        }

        public IEnumerable<ADL.Analytics.Models.StorageContainer> ListStorageContainers(AnalyticsAccount account, string storage_account)
        {
            return this._rest_client.ListStorageContainers(account.ResourceGroup, account.Name, storage_account);
        }

        public void DeleteStorageAccount(AnalyticsAccount account, string storage_account)
        {
            this._rest_client.DeleteStorageAccount(account.ResourceGroup, account.Name, storage_account);
        }

        public void DeleteDataLakeStoreAccount(AnalyticsAccount account, string storage_account)
        {
            this._rest_client.DeleteDataLakeStoreAccount(account.ResourceGroup, account.Name, storage_account);
        }

        public IEnumerable<ADL.Analytics.Models.SasTokenInfo> ListSasTokens(AnalyticsAccount account, string storage_account, string container)
        {
            return this._rest_client.ListSasTokens(account.ResourceGroup, account.Name, storage_account, container);
        }
    }
}

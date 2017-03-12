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
            return this._rest_client.GetAccount(account.ResourceGroup, account.GetUri());
        }

        public bool ExistsAccount(AnalyticsAccount account)
        {
            return this._rest_client.ExistsAccount(account.ResourceGroup, account.GetUri());
        }

        public void UpdateAccount(AnalyticsAccount account, ADL.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this._rest_client.UpdateAccount(account.ResourceGroup, account.GetUri(), parameters);
        }
   
        public void LinkBlobStorageAccount(AnalyticsAccount account, string storage_account, ADL.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._rest_client.AddStorageAccount(account.ResourceGroup, account.GetUri(), storage_account, parameters);
        }

        public void LinkDataLakeStoreAccount(AnalyticsAccount account, string storage_account, ADL.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._rest_client.AddDataLakeStoreAccount(account.ResourceGroup, account.GetUri(), storage_account, parameters);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeStoreAccountInfo> ListLinkedDataLakeStoreAccounts(AnalyticsAccount account)
        {
            return this._rest_client.ListStoreAccounts(account.ResourceGroup, account.GetUri());
        }

        public IEnumerable<ADL.Analytics.Models.StorageAccountInfo> ListLinkedBlobStorageAccounts(AnalyticsAccount account)
        {
            return this._rest_client.ListStorageAccounts(account.ResourceGroup, account.GetUri());
        }

        public IEnumerable<ADL.Analytics.Models.StorageContainer> ListLinkedBlobStorageContainers(AnalyticsAccount account, string storage_account)
        {
            return this._rest_client.ListStorageContainers(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public void UnlinkBlobStorageAccount(AnalyticsAccount account, string storage_account)
        {
            this._rest_client.DeleteStorageAccount(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public void UnlinkDataLakeStoreAccount(AnalyticsAccount account, string storage_account)
        {
            this._rest_client.DeleteDataLakeStoreAccount(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public IEnumerable<ADL.Analytics.Models.SasTokenInfo> ListBlobStorageSasTokens(AnalyticsAccount account, string storage_account, string container)
        {
            return this._rest_client.ListSasTokens(account.ResourceGroup, account.GetUri(), storage_account, container);
        }
    }
}

using System.Collections.Generic;
using AzureDataLakeClient.Authentication;

namespace AzureDataLakeClient.Analytics
{
    public class ManagementCommands
    {
        private AnalyticsRmRestClient _rest_client;
        private AnalyticsAccount account;
        AuthenticatedSession authSession;

        public ManagementCommands(AnalyticsAccount a, AnalyticsRmRestClient c, AuthenticatedSession authSession)
        {
            this.account = a;
            this._rest_client = c;
            this.authSession = authSession;
        }

        public void UpdateAccount(Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this._rest_client.UpdateAccount(this.account.ResourceGroup, account.GetUri(), parameters);
        }

        public void LinkBlobStorageAccount(string storage_account, Microsoft.Azure.Management.DataLake.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._rest_client.AddStorageAccount(this.account.ResourceGroup, account.GetUri(), storage_account, parameters);
        }

        public void LinkDataLakeStoreAccount(string storage_account, Microsoft.Azure.Management.DataLake.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._rest_client.AddDataLakeStoreAccount(this.account.ResourceGroup, account.GetUri(), storage_account, parameters);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeStoreAccountInfo> ListLinkedDataLakeStoreAccounts()
        {
            return this._rest_client.ListStoreAccounts(this.account.ResourceGroup, account.GetUri());
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.StorageAccountInfo> ListLinkedBlobStorageAccounts()
        {
            return this._rest_client.ListStorageAccounts(account.ResourceGroup, account.GetUri());
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.StorageContainer> ListLinkedBlobStorageContainers(string storage_account)
        {
            return this._rest_client.ListStorageContainers(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public void UnlinkBlobStorageAccount(string storage_account)
        {
            this._rest_client.DeleteStorageAccount(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public void UnlinkDataLakeStoreAccount(string storage_account)
        {
            this._rest_client.DeleteDataLakeStoreAccount(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.SasTokenInfo> ListBlobStorageSasTokens(string storage_account, string container)
        {
            return this._rest_client.ListSasTokens(account.ResourceGroup, account.GetUri(), storage_account, container);
        }

    }
}
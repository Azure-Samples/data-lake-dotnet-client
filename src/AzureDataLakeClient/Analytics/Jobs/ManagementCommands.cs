using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;

namespace AzureDataLakeClient.Analytics.Jobs
{
    public class ManagementCommands
    {
        private AnalyticsAccountManagmentRestWrapper _restClientWrapper;
        private AnalyticsAccount account;
        AuthenticatedSession authSession;
        public ManagementCommands(AnalyticsAccount a, AnalyticsAccountManagmentRestWrapper c)
        {
            this.account = a;
            this._restClientWrapper = c;
        }

        public void UpdateAccount(Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this._restClientWrapper.UpdateAccount(this.account.ResourceGroup, account.GetUri(), parameters);
        }

        public void LinkBlobStorageAccount(string storage_account, Microsoft.Azure.Management.DataLake.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._restClientWrapper.AddStorageAccount(this.account.ResourceGroup, account.GetUri(), storage_account, parameters);
        }

        public void LinkDataLakeStoreAccount(string storage_account, Microsoft.Azure.Management.DataLake.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._restClientWrapper.AddDataLakeStoreAccount(this.account.ResourceGroup, account.GetUri(), storage_account, parameters);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeStoreAccountInfo> ListLinkedDataLakeStoreAccounts()
        {
            return this._restClientWrapper.ListStoreAccounts(this.account.ResourceGroup, account.GetUri());
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.StorageAccountInfo> ListLinkedBlobStorageAccounts()
        {
            return this._restClientWrapper.ListStorageAccounts(account.ResourceGroup, account.GetUri());
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.StorageContainer> ListLinkedBlobStorageContainers(string storage_account)
        {
            return this._restClientWrapper.ListStorageContainers(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public void UnlinkBlobStorageAccount(string storage_account)
        {
            this._restClientWrapper.DeleteStorageAccount(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public void UnlinkDataLakeStoreAccount(string storage_account)
        {
            this._restClientWrapper.DeleteDataLakeStoreAccount(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.SasTokenInfo> ListBlobStorageSasTokens(string storage_account, string container)
        {
            return this._restClientWrapper.ListSasTokens(account.ResourceGroup, account.GetUri(), storage_account, container);
        }

    }
}
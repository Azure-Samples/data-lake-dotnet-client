using System.Collections.Generic;
using AzureDataLakeClient.Authentication;

namespace AzureDataLakeClient.Jobs
{
    public class ManagementCommands
    {
        private AnalyticsAccount account;
        public AnalyticsRestClients clients;

        public ManagementCommands(AnalyticsAccount a, AnalyticsRestClients c)
        {
            this.account = a;
            this.clients = c;
        }

        public void UpdateAccount(Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this.clients._AdlaAccountMgmtRest.UpdateAccount(this.account.ResourceGroup, account, parameters);
        }

        public void LinkBlobStorageAccount(string storage_account, Microsoft.Azure.Management.DataLake.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this.clients._AdlaAccountMgmtRest.AddStorageAccount(this.account.ResourceGroup, account, storage_account, parameters);
        }

        public void LinkDataLakeStoreAccount(string storage_account, Microsoft.Azure.Management.DataLake.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this.clients._AdlaAccountMgmtRest.AddDataLakeStoreAccount(this.account.ResourceGroup, account, storage_account, parameters);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeStoreAccountInfo> ListLinkedDataLakeStoreAccounts()
        {
            return this.clients._AdlaAccountMgmtRest.ListStoreAccounts(this.account.ResourceGroup, account);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.StorageAccountInfo> ListLinkedBlobStorageAccounts()
        {
            return this.clients._AdlaAccountMgmtRest.ListStorageAccounts(account.ResourceGroup, account);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.StorageContainer> ListLinkedBlobStorageContainers(string storage_account)
        {
            return this.clients._AdlaAccountMgmtRest.ListStorageContainers(account.ResourceGroup, account, storage_account);
        }

        public void UnlinkBlobStorageAccount(string storage_account)
        {
            this.clients._AdlaAccountMgmtRest.DeleteStorageAccount(account.ResourceGroup, account, storage_account);
        }

        public void UnlinkDataLakeStoreAccount(string storage_account)
        {
            this.clients._AdlaAccountMgmtRest.DeleteDataLakeStoreAccount(account.ResourceGroup, account, storage_account);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.SasTokenInfo> ListBlobStorageSasTokens(string storage_account, string container)
        {
            return this.clients._AdlaAccountMgmtRest.ListSasTokens(account.ResourceGroup, account, storage_account, container);
        }

    }
}
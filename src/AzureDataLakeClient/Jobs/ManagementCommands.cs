using System.Collections.Generic;

namespace AzureDataLakeClient.Jobs
{
    public class ManagementCommands
    {
        private readonly AnalyticsAccount AnalyticsAccount;
        public readonly AnalyticsRestClients RestClients;

        public ManagementCommands(AnalyticsAccount account, AnalyticsRestClients restclients)
        {
            this.AnalyticsAccount = account;
            this.RestClients = restclients;
        }

        public void UpdateAccount(Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this.RestClients._AdlaAccountMgmtRest.UpdateAccount(this.AnalyticsAccount.ResourceGroup, AnalyticsAccount, parameters);
        }

        public void LinkBlobStorageAccount(string storage_account, Microsoft.Azure.Management.DataLake.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this.RestClients._AdlaAccountMgmtRest.AddStorageAccount(this.AnalyticsAccount.ResourceGroup, AnalyticsAccount, storage_account, parameters);
        }

        public void LinkDataLakeStoreAccount(string storage_account, Microsoft.Azure.Management.DataLake.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this.RestClients._AdlaAccountMgmtRest.AddDataLakeStoreAccount(this.AnalyticsAccount.ResourceGroup, AnalyticsAccount, storage_account, parameters);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeStoreAccountInfo> ListLinkedDataLakeStoreAccounts()
        {
            return this.RestClients._AdlaAccountMgmtRest.ListStoreAccounts(this.AnalyticsAccount.ResourceGroup, AnalyticsAccount);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.StorageAccountInfo> ListLinkedBlobStorageAccounts()
        {
            return this.RestClients._AdlaAccountMgmtRest.ListStorageAccounts(AnalyticsAccount.ResourceGroup, AnalyticsAccount);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.StorageContainer> ListLinkedBlobStorageContainers(string storage_account)
        {
            return this.RestClients._AdlaAccountMgmtRest.ListStorageContainers(AnalyticsAccount.ResourceGroup, AnalyticsAccount, storage_account);
        }

        public void UnlinkBlobStorageAccount(string storage_account)
        {
            this.RestClients._AdlaAccountMgmtRest.DeleteStorageAccount(AnalyticsAccount.ResourceGroup, AnalyticsAccount, storage_account);
        }

        public void UnlinkDataLakeStoreAccount(string storage_account)
        {
            this.RestClients._AdlaAccountMgmtRest.DeleteDataLakeStoreAccount(AnalyticsAccount.ResourceGroup, AnalyticsAccount, storage_account);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.SasTokenInfo> ListBlobStorageSasTokens(string storage_account, string container)
        {
            return this.RestClients._AdlaAccountMgmtRest.ListSasTokens(AnalyticsAccount.ResourceGroup, AnalyticsAccount, storage_account, container);
        }

    }
}
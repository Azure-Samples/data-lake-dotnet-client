using System.Collections.Generic;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Jobs
{
    public class LinkedStorageCommands
    {
        public readonly AnalyticsAccount AnalyticsAccount;
        public readonly AnalyticsRestClients RestClients;

        public LinkedStorageCommands(AnalyticsAccount account, AnalyticsRestClients restclients)
        {
            this.AnalyticsAccount = account;
            this.RestClients = restclients;
        }

        public void LinkBlobStorageAccount(string storage_account, MSADLA.Models.AddStorageAccountParameters parameters)
        {
            this.RestClients._AdlaAccountMgmtRest.AddStorageAccount(this.AnalyticsAccount.ResourceGroup,
                AnalyticsAccount, storage_account, parameters);
        }

        public void LinkDataLakeStoreAccount(string storage_account, MSADLA.Models.AddDataLakeStoreParameters parameters)
        {
            this.RestClients._AdlaAccountMgmtRest.AddDataLakeStoreAccount(this.AnalyticsAccount.ResourceGroup,
                AnalyticsAccount, storage_account, parameters);
        }

        public IEnumerable<MSADLA.Models.DataLakeStoreAccountInfo> ListDataLakeStoreAccounts()
        {
            return this.RestClients._AdlaAccountMgmtRest.ListStoreAccounts(AnalyticsAccount);
        }

        public IEnumerable<MSADLA.Models.StorageAccountInfo> ListBlobStorageAccounts()
        {
            return this.RestClients._AdlaAccountMgmtRest.ListStorageAccounts(AnalyticsAccount);
        }

        public IEnumerable<MSADLA.Models.StorageContainer> ListBlobStorageContainers(string storage_account)
        {
            return this.RestClients._AdlaAccountMgmtRest.ListStorageContainers(AnalyticsAccount, storage_account);
        }

        public void UnlinkBlobStorageAccount(string storage_account)
        {
            this.RestClients._AdlaAccountMgmtRest.DeleteStorageAccount(AnalyticsAccount, storage_account);
        }

        public void UnlinkDataLakeStoreAccount(string storage_account)
        {
            this.RestClients._AdlaAccountMgmtRest.DeleteDataLakeStoreAccount(AnalyticsAccount, storage_account);
        }

        public IEnumerable<MSADLA.Models.SasTokenInfo> ListBlobStorageSasTokens(string storage_account, string container)
        {
            return this.RestClients._AdlaAccountMgmtRest.ListSasTokens(AnalyticsAccount, storage_account, container);
        }
    }
}
using System.Collections.Generic;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Commands
{
    public class LinkedStoreCommands
    {
        internal readonly AdlClient.Models.AnalyticsAccountRef Account;
        internal readonly AnalyticsRestClients RestClients;

        internal LinkedStoreCommands(AdlClient.Models.AnalyticsAccountRef account, AnalyticsRestClients restclients)
        {
            this.Account = account;
            this.RestClients = restclients;
        }

        public void LinkBlobStorageAccount(string account, MSADLA.Models.AddStorageAccountParameters parameters)
        {
            this.RestClients._AdlaAccountMgmtRest.AddStorageAccount(Account, account, parameters);
        }

        public void LinkDataLakeStoreAccount(string storage_account, MSADLA.Models.AddDataLakeStoreParameters parameters)
        {
            this.RestClients._AdlaAccountMgmtRest.AddDataLakeStoreAccount(Account, storage_account, parameters);
        }

        public IEnumerable<MSADLA.Models.DataLakeStoreAccountInfo> ListDataLakeStoreAccounts()
        {
            return this.RestClients._AdlaAccountMgmtRest.ListStoreAccounts(Account);
        }

        public IEnumerable<MSADLA.Models.StorageAccountInfo> ListBlobStorageAccounts()
        {
            return this.RestClients._AdlaAccountMgmtRest.ListStorageAccounts(Account);
        }

        public IEnumerable<MSADLA.Models.StorageContainer> ListBlobStorageContainers(string storage_account)
        {
            return this.RestClients._AdlaAccountMgmtRest.ListStorageContainers(Account, storage_account);
        }

        public void UnlinkBlobStorageAccount(string storage_account)
        {
            this.RestClients._AdlaAccountMgmtRest.DeleteStorageAccount(Account, storage_account);
        }

        public void UnlinkDataLakeStoreAccount(string storage_account)
        {
            this.RestClients._AdlaAccountMgmtRest.DeleteDataLakeStoreAccount(Account, storage_account);
        }

        public IEnumerable<MSADLA.Models.SasTokenInfo> ListBlobStorageSasTokens(string storage_account, string container)
        {
            return this.RestClients._AdlaAccountMgmtRest.ListSasTokens(Account, storage_account, container);
        }
    }
}
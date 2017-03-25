using MSADLS = Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient.Commands
{
    public class StoreAccountCommands
    {
        public readonly StoreAccountRef StoreAccount;
        public readonly StoreRestClients RestClients;

        public StoreAccountCommands(StoreAccountRef account, StoreRestClients restclients)
        {
            this.StoreAccount = account;
            this.RestClients = restclients;
        }

        public MSADLS.Models.DataLakeStoreAccount Get()
        {
            var acc = this.RestClients.StoreAccountMgmtRest.GetAccount(this.StoreAccount);
            return acc;
        }

        public void Update(MSADLS.Models.DataLakeStoreAccountUpdateParameters parameters)
        {
            this.RestClients.StoreAccountMgmtRest.Update(this.StoreAccount, parameters);
        }
    }
}
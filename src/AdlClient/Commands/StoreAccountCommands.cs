using MSADLS = Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient.Commands
{
    public class StoreAccountCommands
    {
        public readonly StoreAccountRef Account;
        public readonly StoreRestClients RestClients;

        public StoreAccountCommands(StoreAccountRef account, StoreRestClients restclients)
        {
            this.Account = account;
            this.RestClients = restclients;
        }

        public MSADLS.Models.DataLakeStoreAccount Get()
        {
            var acc = this.RestClients.StoreAccountMgmtRest.GetAccount(this.Account);
            return acc;
        }

        public void Update(MSADLS.Models.DataLakeStoreAccountUpdateParameters parameters)
        {
            this.RestClients.StoreAccountMgmtRest.Update(this.Account, parameters);
        }
    }
}
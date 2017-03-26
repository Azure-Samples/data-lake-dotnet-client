using MSADLS = Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient.Commands
{
    public class StoreAccountCommands
    {
        internal readonly StoreAccountRef Account;
        internal readonly StoreRestClients RestClients;

        internal StoreAccountCommands(StoreAccountRef account, StoreRestClients restclients)
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
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

        public void Update(Microsoft.Azure.Management.DataLake.Store.Models.DataLakeStoreAccountUpdateParameters parameters)
        {
            this.RestClients.StoreAccountMgmtRest.Update(this.StoreAccount, parameters);
        }

    }
}
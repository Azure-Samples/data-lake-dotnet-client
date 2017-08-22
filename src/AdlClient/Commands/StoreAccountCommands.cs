using Microsoft.Azure.Management.DataLake.Store;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient.Commands
{
    public class StoreAccountCommands
    {
        internal readonly AdlClient.Models.StoreAccountRef Account;
        internal readonly AdlClient.Rest.StoreRestClients RestClients;

        internal StoreAccountCommands(AdlClient.Models.StoreAccountRef account, AdlClient.Rest.StoreRestClients restclients)
        {
            this.Account = account;
            this.RestClients = restclients;
        }

        public MSADLS.Models.DataLakeStoreAccount Get()
        {
            var acc = this.RestClients.AccountClient.Account.Get(this.Account.ResourceGroup, this.Account.Name);
            return acc;
        }

        public void Update(MSADLS.Models.DataLakeStoreAccountUpdateParameters parameters)
        {
            this.RestClients.AccountClient.Account.Update(this.Account.ResourceGroup, this.Account.Name, parameters);
        }

        public void Delete()
        {
            this.RestClients.AccountClient.Account.Delete(this.Account.ResourceGroup, this.Account.Name);
        }

        public bool Exists()
        {
            return this.RestClients.AccountClient.Account.Exists(this.Account.ResourceGroup, this.Account.Name);
        }

    }
}
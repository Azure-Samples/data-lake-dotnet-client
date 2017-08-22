using Microsoft.Azure.Management.DataLake.Analytics;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Commands
{
    public class AnalyticsAccountCommands
    {
        internal readonly AdlClient.Models.AnalyticsAccountRef Account;
        internal readonly AnalyticsRestClients RestClients;

        public  readonly LinkedStoreCommands LinkedStorage;

        internal AnalyticsAccountCommands(AdlClient.Models.AnalyticsAccountRef account, AnalyticsRestClients restclients)
        {
            this.Account = account;
            this.RestClients = restclients;
            this.LinkedStorage  = new LinkedStoreCommands(account, restclients);
        }

        public MSADLA.Models.DataLakeAnalyticsAccount Get()
        {
            var adls_account = this.RestClients.Account.Account.Get(this.Account.ResourceGroup, this.Account.Name);
            return adls_account;
        }

        public void Update( MSADLA.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this.RestClients.Account.Account.Update(this.Account.ResourceGroup, this.Account.Name, parameters);
        }

        public bool Exists()
        {
            return this.RestClients.Account.Account.Exists(this.Account.ResourceGroup, this.Account.Name);
        }
    }
}
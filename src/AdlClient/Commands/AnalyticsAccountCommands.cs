using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Commands
{
    public class AnalyticsAccountCommands
    {
        public readonly AnalyticsAccountRef AnalyticsAccount;
        public readonly AnalyticsRestClients RestClients;
        public readonly LinkedStoreCommands LinkedStorage;

        public AnalyticsAccountCommands(AnalyticsAccountRef account, AnalyticsRestClients restclients)
        {
            this.AnalyticsAccount = account;
            this.RestClients = restclients;
            this.LinkedStorage  = new LinkedStoreCommands(account,restclients);
        }

        public MSADLA.Models.DataLakeAnalyticsAccount Get()
        {
            var acc = this.RestClients._AdlaAccountMgmtRest.GetAccount(this.AnalyticsAccount);
            return acc;
        }

        public void Update( MSADLA.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this.RestClients._AdlaAccountMgmtRest.UpdateAccount(this.AnalyticsAccount, parameters);
        }
    }
}
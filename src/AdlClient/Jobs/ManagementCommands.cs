namespace AdlClient.Jobs
{
    public class ManagementCommands
    {
        public readonly AnalyticsAccount AnalyticsAccount;
        public readonly AnalyticsRestClients RestClients;
        public readonly LinkedStorageCommands LinkedStorage;

        public ManagementCommands(AnalyticsAccount account, AnalyticsRestClients restclients)
        {
            this.AnalyticsAccount = account;
            this.RestClients = restclients;
            this.LinkedStorage = new LinkedStorageCommands(account,restclients);
        }

        public void UpdateAccount(
            Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this.RestClients._AdlaAccountMgmtRest.UpdateAccount(this.AnalyticsAccount.ResourceGroup, AnalyticsAccount,
                parameters);
        }

    }
}
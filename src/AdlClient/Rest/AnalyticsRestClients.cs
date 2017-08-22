using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient
{
    public class AnalyticsRestClients
    {
        public readonly MSADLA.DataLakeAnalyticsJobManagementClient JobsClient;
        public readonly MSADLA.DataLakeAnalyticsCatalogManagementClient CatalogClient;
        public readonly MSADLA.DataLakeAnalyticsAccountManagementClient AccountClient;
        public readonly AdlClient.Models.AnalyticsAccountRef Account;

        public AnalyticsRestClients(Authentication authSession, AdlClient.Models.AnalyticsAccountRef account)
        {
            this.Account = account;
            this.JobsClient = new MSADLA.DataLakeAnalyticsJobManagementClient(authSession.ADLCreds);
            this.CatalogClient = new MSADLA.DataLakeAnalyticsCatalogManagementClient(authSession.ADLCreds);
            this.AccountClient = new MSADLA.DataLakeAnalyticsAccountManagementClient(authSession.ARMCreds);
            this.AccountClient.SubscriptionId = account.SubscriptionId;
        }
    }
}
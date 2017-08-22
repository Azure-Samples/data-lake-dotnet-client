using AdlClient.Rest;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;
namespace AdlClient
{
    public class AnalyticsRestClients
    {
        public readonly AnalyticsAccountManagmentRestWrapper _AdlaAccountMgmtRest;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient AADclient;

        public readonly MSADLA.DataLakeAnalyticsJobManagementClient Jobs;
        public readonly MSADLA.DataLakeAnalyticsCatalogManagementClient Catalog;
        public readonly MSADLA.DataLakeAnalyticsAccountManagementClient Account;

        public AnalyticsRestClients(Authentication authSession, AdlClient.Models.AnalyticsAccountRef account)
        {
            this._AdlaAccountMgmtRest = new AnalyticsAccountManagmentRestWrapper(account.SubscriptionId, authSession.ARMCreds);
            this.AADclient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(authSession.AADCreds);
            this.AADclient.TenantID = authSession.Tenant;

            this.Jobs = new MSADLA.DataLakeAnalyticsJobManagementClient(authSession.ADLCreds);
            this.Catalog = new MSADLA.DataLakeAnalyticsCatalogManagementClient(authSession.ADLCreds);
            this.Account = new MSADLA.DataLakeAnalyticsAccountManagementClient(authSession.ARMCreds);
            this.Account.SubscriptionId = account.SubscriptionId;
        }
    }
}
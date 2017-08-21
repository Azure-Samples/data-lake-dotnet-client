using AdlClient.Rest;

namespace AdlClient
{
    public class AnalyticsRestClients
    {
        public readonly AnalyticsJobsRestWrapper _JobRest;
        public readonly AnalyticsCatalogRestWrapper _CatalogRest;
        public readonly AnalyticsAccountManagmentRestWrapper _AdlaAccountMgmtRest;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient AADclient;

        public AnalyticsRestClients(InteractiveAuthentication authSession, AdlClient.Models.AnalyticsAccountRef account)
        {
            this._JobRest = new AnalyticsJobsRestWrapper(authSession.AdlCreds);
            this._CatalogRest = new AnalyticsCatalogRestWrapper(authSession.AdlCreds);
            this._AdlaAccountMgmtRest = new AnalyticsAccountManagmentRestWrapper(account.SubscriptionId, authSession.ArmCreds);
            this.AADclient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(authSession.AadCreds);
            this.AADclient.TenantID = authSession.Tenant;
        }
    }
}
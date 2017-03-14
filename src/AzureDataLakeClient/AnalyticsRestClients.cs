using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;

namespace AzureDataLakeClient
{
    public class AnalyticsRestClients
    {
        public readonly AnalyticsJobsRestWrapper _JobRest;
        public readonly AnalyticsCatalogRestWrapper _CatalogRest;
        public readonly AnalyticsAccountManagmentRestWrapper _AdlaAccountMgmtRest;

        public AnalyticsRestClients(AnalyticsAccount account, AuthenticatedSession authSession)
        {
            this._JobRest = new AnalyticsJobsRestWrapper(authSession.Credentials);
            this._CatalogRest = new AnalyticsCatalogRestWrapper(authSession.Credentials);
            this._AdlaAccountMgmtRest = new AnalyticsAccountManagmentRestWrapper(account.Subscription, authSession.Credentials);
        }
    }
}
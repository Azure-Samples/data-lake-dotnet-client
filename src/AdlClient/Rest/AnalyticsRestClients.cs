using AdlClient.Rest;

namespace AdlClient
{
    public class AnalyticsRestClients
    {
        public readonly AnalyticsJobsRestWrapper _JobRest;
        public readonly AnalyticsCatalogRestWrapper _CatalogRest;
        public readonly AnalyticsAccountManagmentRestWrapper _AdlaAccountMgmtRest;

        public AnalyticsRestClients(Authentication authSession, AnalyticsAccountRef account)
        {
            this._JobRest = new AnalyticsJobsRestWrapper(authSession.Credentials);
            this._CatalogRest = new AnalyticsCatalogRestWrapper(authSession.Credentials);
            this._AdlaAccountMgmtRest = new AnalyticsAccountManagmentRestWrapper(account.SubscriptionId, authSession.Credentials);
        }
    }
}
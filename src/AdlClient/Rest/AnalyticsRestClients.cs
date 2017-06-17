using AdlClient.Rest;

namespace AdlClient
{
    public class AnalyticsRestClients
    {
        public readonly AnalyticsJobsRestWrapper _JobRest;
        public readonly AnalyticsCatalogRestWrapper _CatalogRest;
        public readonly AnalyticsAccountManagmentRestWrapper _AdlaAccountMgmtRest;

        public AnalyticsRestClients(Authentication authSession, AdlClient.Models.AnalyticsAccountRef account)
        {
            this._JobRest = new AnalyticsJobsRestWrapper(authSession.ADLCreds);
            this._CatalogRest = new AnalyticsCatalogRestWrapper(authSession.ADLCreds);
            this._AdlaAccountMgmtRest = new AnalyticsAccountManagmentRestWrapper(account.SubscriptionId, authSession.ARMCreds);
        }
    }
}
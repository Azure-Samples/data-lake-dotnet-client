using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Catalog;
using AzureDataLakeClient.Jobs;
using AzureDataLakeClient.Rest;

namespace AzureDataLakeClient
{
    public class AnalyticsClient : AzureDataLakeClient.ClientBase
    {
        private AnalyticsJobsRestWrapper _JobRest;
        private AnalyticsCatalogRestWrapper _CatalogRest;
        private AnalyticsAccountManagmentRestWrapper _AdlaAccountMgmtRest;

        public readonly JobCommands Jobs;
        public readonly CatalogCommands Catalog;
        public readonly ManagementCommands Management;

        public AnalyticsClient(AnalyticsAccount account, AuthenticatedSession authSession) :
            base(authSession)
        {
            this._JobRest = new AnalyticsJobsRestWrapper(this.AuthenticatedSession.Credentials);
            this._CatalogRest = new AnalyticsCatalogRestWrapper(this.AuthenticatedSession.Credentials);
            this._AdlaAccountMgmtRest = new AnalyticsAccountManagmentRestWrapper(account.Subscription, authSession.Credentials);

            this.Jobs = new JobCommands(account, this._JobRest);
            this.Catalog = new CatalogCommands(account, this._CatalogRest);
            this.Management = new ManagementCommands(account, this._AdlaAccountMgmtRest);
        }
    }
}
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Catalog;
using AzureDataLakeClient.Jobs;
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

    public class AnalyticsClient : AzureDataLakeClient.ClientBase
    {
        public readonly JobCommands Jobs;
        public readonly CatalogCommands Catalog;
        public readonly ManagementCommands Management;

        public AnalyticsRestClients RestClients;

        public AnalyticsClient(AnalyticsAccount account, AuthenticatedSession authSession) :
            base(authSession)
        {
            this.RestClients = new AnalyticsRestClients(account, authSession);

            this.Jobs = new JobCommands(account, this.RestClients);
            this.Catalog = new CatalogCommands(account, this.RestClients);
            this.Management = new ManagementCommands(account, this.RestClients);
        }        
    }
}
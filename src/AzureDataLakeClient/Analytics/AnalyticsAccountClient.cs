using AzureDataLakeClient.Analytics.Clients;
using AzureDataLakeClient.Analytics.Commands;
using AzureDataLakeClient.Authentication;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsAccountClient : AccountClientBase
    {
        private AnalyticsJobsRestClient _adla_job_rest_client;
        private AnalyticsCatalogRestClient _adla_catalog_rest_client;
        private AnalyticsAccountManagmentClient _adla_acctmgmt_client;

        public readonly JobCommands Jobs;
        public readonly CatalogCommands Catalog;
        public readonly ManagementCommands Management;

        public AnalyticsAccountClient(AnalyticsAccount account, AuthenticatedSession authSession) :
            base(account.Name, authSession)
        {
            this._adla_job_rest_client = new AnalyticsJobsRestClient(this.AuthenticatedSession.Credentials);
            this._adla_catalog_rest_client = new AnalyticsCatalogRestClient(this.AuthenticatedSession.Credentials);
            this._adla_acctmgmt_client = new AnalyticsAccountManagmentClient(account.Subscription, authSession.Credentials);

            this.Jobs = new JobCommands(account, this._adla_job_rest_client, authSession);
            this.Catalog = new CatalogCommands(account, this._adla_catalog_rest_client);
            this.Management = new ManagementCommands(account, this._adla_acctmgmt_client);
        }
    }
}
using AzureDataLakeClient.Analytics.Catalog;
using AzureDataLakeClient.Analytics.Jobs;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsAccountClient : AccountClientBase
    {
        private AnalyticsJobsRestWrapper _adlaJobRestWrapper;
        private AnalyticsCatalogRestWrapper _adlaCatalogRestClientWrapper;
        private AnalyticsAccountManagmentRestWrapper _adlaAcctmgmtClientWrapper;

        public readonly JobCommands Jobs;
        public readonly CatalogCommands Catalog;
        public readonly ManagementCommands Management;

        public AnalyticsAccountClient(AnalyticsAccount account, AuthenticatedSession authSession) :
            base(account.Name, authSession)
        {
            this._adlaJobRestWrapper = new AnalyticsJobsRestWrapper(this.AuthenticatedSession.Credentials);
            this._adlaCatalogRestClientWrapper = new AnalyticsCatalogRestWrapper(this.AuthenticatedSession.Credentials);
            this._adlaAcctmgmtClientWrapper = new AnalyticsAccountManagmentRestWrapper(account.Subscription, authSession.Credentials);

            this.Jobs = new JobCommands(account, this._adlaJobRestWrapper);
            this.Catalog = new CatalogCommands(account, this._adlaCatalogRestClientWrapper);
            this.Management = new ManagementCommands(account, this._adlaAcctmgmtClientWrapper);
        }
    }
}
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;

namespace AzureDataLakeClient
{
    public class ResourceClient: ClientBase
    {
        private readonly AnalyticsAccountManagmentRestWrapper _adlaAccountMgmtClientWrapper;
        private readonly StoreManagementRestWrapper _adls_account_mgmt_client;
        public readonly Subscription Subscription;

        public AnalyticsResourceCommands Analytics;
        public StoreResourceCommands Store;

        public ResourceClient(Subscription subscription, AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Subscription = subscription;
            this._adlaAccountMgmtClientWrapper = new AnalyticsAccountManagmentRestWrapper(subscription, authSession.Credentials);
            this._adls_account_mgmt_client = new StoreManagementRestWrapper(subscription, authSession.Credentials);

            this.Analytics = new AnalyticsResourceCommands(subscription, authSession, _adlaAccountMgmtClientWrapper);
            this.Store = new StoreResourceCommands(subscription,authSession,this._adls_account_mgmt_client);
        }
    }
}

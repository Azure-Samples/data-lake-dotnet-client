using AzureDataLakeClient.Store;
using AzureDataLakeClient.Store.Clients;

namespace ADL_Client_Tests
{
    public class Base_Tests
    {
        private bool init;

        public AzureDataLakeClient.Authentication.AuthenticatedSession auth_session;

        public AzureDataLakeClient.Analytics.AnalyticsAccountClient adla_account_client;
        public StoreAccountClient adls_account_client;
        
        public AzureDataLakeClient.SubscriptionClient sub_client;
        public AzureDataLakeClient.Rm.Subscription sub;
        public AzureDataLakeClient.Rm.ResourceGroup rg;

        public void Initialize()
        {
            if (this.init == false)
            {
                var tenant = new AzureDataLakeClient.Authentication.Tenant("microsoft.onmicrosoft.com");
                this.auth_session = new AzureDataLakeClient.Authentication.AuthenticatedSession(tenant);
                auth_session.Authenticate();

                this.sub = new AzureDataLakeClient.Rm.Subscription("045c28ea-c686-462f-9081-33c34e871ba3");
                this.rg = new AzureDataLakeClient.Rm.ResourceGroup("InsightsServices");

                var store_account = new AzureDataLakeClient.Store.StoreUri("datainsightsadhoc");
                var analytics_account = new AzureDataLakeClient.Analytics.AnalyticsAccount("datainsightsadhoc", sub, rg);
                this.init = true;

                this.adls_account_client = new StoreAccountClient(store_account, auth_session);
                this.adla_account_client = new AzureDataLakeClient.Analytics.AnalyticsAccountClient(analytics_account, auth_session);
                this.sub_client = new AzureDataLakeClient.SubscriptionClient(sub, auth_session);

            }
        }

    }
}

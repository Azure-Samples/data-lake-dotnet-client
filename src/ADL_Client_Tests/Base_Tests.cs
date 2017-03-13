using ADLC = AzureDataLakeClient;
using MS_ADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace ADL_Client_Tests
{
    public class Base_Tests
    {
        private bool init;

        public ADLC.Authentication.AuthenticatedSession auth_session;

        public ADLC.Analytics.AnalyticsAccountClient adla_account_client;
        public ADLC.Store.StoreAccountClient adls_account_client;
        
        public ADLC.Rm.SubscriptionClient sub_client;
        public ADLC.Rm.Subscription sub;
        public ADLC.Rm.ResourceGroup rg;

        public void Initialize()
        {
            if (this.init == false)
            {
                var tenant = new ADLC.Authentication.Tenant("microsoft.onmicrosoft.com");
                this.auth_session = new ADLC.Authentication.AuthenticatedSession(tenant);
                auth_session.Authenticate();

                this.sub = new ADLC.Rm.Subscription("045c28ea-c686-462f-9081-33c34e871ba3");
                this.rg = new ADLC.Rm.ResourceGroup("InsightsServices");

                var store_account = new ADLC.Store.StoreAccount("datainsightsadhoc",sub,rg);
                var analytics_account = new ADLC.Analytics.AnalyticsAccount("datainsightsadhoc", sub, rg);
                this.init = true;

                this.adls_account_client = new ADLC.Store.StoreAccountClient(store_account, auth_session);
                this.adla_account_client = new AzureDataLakeClient.Analytics.AnalyticsAccountClient(analytics_account, auth_session);
                this.sub_client = new ADLC.Rm.SubscriptionClient(sub, auth_session);

            }
        }
    }
}

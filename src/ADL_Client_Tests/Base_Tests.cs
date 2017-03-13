using ADLC = AzureDataLakeClient;
using MS_ADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace ADL_Client_Tests
{
    public class Base_Tests
    {
        private bool init;

        public ADLC.Authentication.AuthenticatedSession auth_session;

        public ADLC.AnalyticsClient adla_account_client;
        public ADLC.StoreClient AdlsClient;
        
        public ADLC.ResourceClient sub_client;
        public ADLC.Subscription sub;
        public ADLC.ResourceGroup rg;

        public void Initialize()
        {
            if (this.init == false)
            {
                var tenant = new ADLC.Authentication.Tenant("microsoft.onmicrosoft.com");
                this.auth_session = new ADLC.Authentication.AuthenticatedSession(tenant);
                auth_session.Authenticate();

                this.sub = new ADLC.Subscription("045c28ea-c686-462f-9081-33c34e871ba3");
                this.rg = new ADLC.ResourceGroup("InsightsServices");

                var store_account = new ADLC.StoreAccount("datainsightsadhoc",sub,rg);
                var analytics_account = new ADLC.AnalyticsAccount("datainsightsadhoc", sub, rg);
                this.init = true;

                this.AdlsClient = new ADLC.StoreClient(store_account, auth_session);
                this.adla_account_client = new ADLC.AnalyticsClient(analytics_account, auth_session);
                this.sub_client = new ADLC.ResourceClient(sub, auth_session);

            }
        }
    }
}

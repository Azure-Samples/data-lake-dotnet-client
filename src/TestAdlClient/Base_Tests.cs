using ADLC = AdlClient;

namespace TestAdlClient
{
    public class Base_Tests
    {
        private bool init;

        public ADLC.Authentication Authentication;
        public ADLC.AnalyticsClient AnalyticsClient;
        public ADLC.StoreClient StoreClient;        
        public ADLC.AzureClient AzureClient;
        public string SubscriptionId;
        public string ResourceGroup;

        public void Initialize()
        {
            if (this.init == false)
            {
                string tenant = "microsoft.onmicrosoft.com";
                this.Authentication = new ADLC.Authentication(tenant);
                Authentication.Authenticate();

                this.SubscriptionId = "045c28ea-c686-462f-9081-33c34e871ba3";
                this.ResourceGroup = "InsightsServices";

                this.AzureClient = new AdlClient.AzureClient(this.Authentication);
                this.StoreClient = this.AzureClient.Store.ConnectToAccount(SubscriptionId, ResourceGroup, "datainsightsadhoc");
                this.AnalyticsClient = this.AzureClient.Analytics.ConnectToAccount(SubscriptionId, ResourceGroup, "datainsightsadhoc");

                this.init = true;

            }
        }
    }
}

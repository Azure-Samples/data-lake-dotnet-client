
namespace TestAdlClient
{
    public class Base_Tests
    {
        private bool init;

        public AdlClient.Authentication Authentication;
        public AdlClient.AnalyticsClient AnalyticsClient;
        public AdlClient.StoreClient StoreClient;        
        public AdlClient.AzureClient AzureClient;
        public string SubscriptionId;
        public string ResourceGroup;

        public void Initialize()
        {
            if (this.init == false)
            {
                string tenant = "microsoft.onmicrosoft.com";
                this.Authentication = new AdlClient.Authentication(tenant);
                Authentication.Authenticate();

                this.SubscriptionId = "045c28ea-c686-462f-9081-33c34e871ba3";
                this.ResourceGroup = "InsightsServices";

                var adla_account = new AdlClient.Models.AnalyticsAccountRef(this.SubscriptionId, this.ResourceGroup, "datainsightsadhoc");
                var adls_account = new AdlClient.Models.StoreAccountRef(this.SubscriptionId, this.ResourceGroup, "datainsightsadhoc");

                this.AzureClient = new AdlClient.AzureClient(this.Authentication);
                this.StoreClient = new AdlClient.StoreClient(this.Authentication, adls_account);
                this.AnalyticsClient = new AdlClient.AnalyticsClient(this.Authentication, adla_account);

                this.init = true;

            }
        }
    }
}

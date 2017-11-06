
namespace TestAdlClient
{
    public class Base_Tests
    {
        private bool init;

        public AdlClient.InteractiveAuthentication Authentication;
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
                this.Authentication = new AdlClient.InteractiveAuthentication(tenant);
                Authentication.Authenticate();

                this.SubscriptionId = "ace74b35-b0de-428b-a1d9-55459d7a6e30";
                this.ResourceGroup = "adlclienttest";

                var adla_account = new AdlClient.Models.AnalyticsAccountRef(this.SubscriptionId, this.ResourceGroup, "adlclientqa");
                var adls_account = new AdlClient.Models.StoreAccountRef(this.SubscriptionId, this.ResourceGroup, "adlclientqa");

                this.AzureClient = new AdlClient.AzureClient(this.Authentication);
                this.StoreClient = new AdlClient.StoreClient(this.Authentication, adls_account);
                this.AnalyticsClient = new AdlClient.AnalyticsClient(this.Authentication, adla_account);

                this.init = true;

            }
        }
    }
}

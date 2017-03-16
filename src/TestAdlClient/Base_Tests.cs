using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADLC = AdlClient;

namespace TestAdlClient
{
    public class Base_Tests
    {
        private bool init;

        public ADLC.Authentication.AuthenticatedSession AuthenticatedSession;
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
                this.AuthenticatedSession = new ADLC.Authentication.AuthenticatedSession(tenant);
                AuthenticatedSession.Authenticate();

                this.SubscriptionId = "045c28ea-c686-462f-9081-33c34e871ba3";
                this.ResourceGroup = "InsightsServices";

                this.AzureClient = new AdlClient.AzureClient(this.AuthenticatedSession);
                this.StoreClient = this.AzureClient.Store.ConnectToAccount(SubscriptionId, ResourceGroup, "datainsightsadhoc");
                this.AnalyticsClient = this.AzureClient.Analytics.ConnectToAccount(SubscriptionId, ResourceGroup, "datainsightsadhoc");

                this.init = true;

            }
        }
    }


    [TestClass]
    public class AzTests : Base_Tests
    {
        [TestMethod]
        public void ListSUbscriptions()
        {
            this.Initialize();
            //foreach (var db in this.AzureClient.ListResourceGroups())
            //{
            //    System.Console.WriteLine("DB {0}", db.Name);
            //}
        }

    }
}

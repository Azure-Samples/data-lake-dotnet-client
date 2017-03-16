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
        public ADLC.Subscription Subscription;
        public ADLC.ResourceGroup ResourceGroup;

        public void Initialize()
        {
            if (this.init == false)
            {
                var tenant = new ADLC.Authentication.Tenant("microsoft.onmicrosoft.com");
                this.AuthenticatedSession = new ADLC.Authentication.AuthenticatedSession(tenant);
                AuthenticatedSession.Authenticate();

                this.Subscription = new ADLC.Subscription("045c28ea-c686-462f-9081-33c34e871ba3");
                this.ResourceGroup = new ADLC.ResourceGroup("InsightsServices");

                var store_account = new ADLC.StoreAccount(Subscription,ResourceGroup, "datainsightsadhoc");
                var analytics_account = new ADLC.AnalyticsAccount(Subscription, ResourceGroup, "datainsightsadhoc");
                this.init = true;

                this.StoreClient = new ADLC.StoreClient(store_account, AuthenticatedSession);
                this.AnalyticsClient = new ADLC.AnalyticsClient(analytics_account, AuthenticatedSession);
                this.AzureClient = new ADLC.AzureClient(AuthenticatedSession);

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

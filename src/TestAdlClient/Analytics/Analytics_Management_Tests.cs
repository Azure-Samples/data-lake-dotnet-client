using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Analytics
{
    [TestClass]
    public class Analytics_Management_Tests : Base_Tests
    {


        [TestMethod]
        public void List_ADLA_Accounts()
        {
            var sub = "045c28ea-c686-462f-9081-33c34e871ba3";

            this.Initialize();
            var adla_accounts = this.AzureClient.Analytics.ListAccountsInSubscription(sub);
            foreach (var a in adla_accounts)
            {
                System.Console.WriteLine("Analytics {0} ", a.Name);
            }

        }
    }
}

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient
{
    [TestClass]
    public class AzTests : Base_Tests
    {
        [TestMethod]
        public void ListSubscriptions()
        {
            this.Initialize();

            var subs = this.AzureClient.ListSubscriptions().ToList();
        }

        [TestMethod]
        public void GetTenantId()
        {
            var t = AdlClient.Authentication.GetTenantId("microsoft.onmicrosoft.com");

        }
    }
}
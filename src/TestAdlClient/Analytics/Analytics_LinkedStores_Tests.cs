using System.Linq;
using AdlClient.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Analytics
{
    [TestClass]
    public class Analytics_LinkedStores_Tests : Base_Tests
    {
        [TestMethod]
        public void Find_Default_ADLS_Account()
        {
            this.Initialize();

            var acct = new AnalyticsAccountRef("ace74b35-b0de-428b-a1d9-55459d7a6e30", "adlpminsights", "adlpm");
            var adla = new AdlClient.AnalyticsClient(this.Authentication, acct);
            var account_info = adla.Account.Get();

            string default_store = account_info.DefaultDataLakeStoreAccount;

        }

    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Analytics
{
    [TestClass]
    public class Analytics_JobRef_Tests : Base_Tests
    {
        [TestMethod]
        public void ParseAccountPortalLink()
        {
            string s =
                "https://portal.azure.com/?feature.customportal=false#resource/subscriptions/ace74b35-b0de-428b-a1d9-55459d7a6e30/resourcegroups/adlpminsights/providers/Microsoft.DataLakeAnalytics/accounts/adlpm/overview";
            var portal_uri = AdlClient.Models.AnalyticsAccountPortalUri.Parse(s);

            Assert.IsNotNull(portal_uri);
            Assert.AreEqual("ace74b35-b0de-428b-a1d9-55459d7a6e30", portal_uri.SubscriptionId);
            Assert.AreEqual("adlpminsights", portal_uri.ResourceGroup);
            Assert.AreEqual("adlpm", portal_uri.Account);
        }

        [TestMethod]
        public void ParseJobPortalLink()
        {
            string s =
                "https://portal.azure.com/?feature.customportal=false#blade/Microsoft_Azure_DataLakeAnalytics/SqlIpJobDetailsBlade/accountId/%2Fsubscriptions%2Face74b35-b0de-428b-a1d9-55459d7a6e30%2Fresourcegroups%2Fadlpminsights%2Fproviders%2FMicrosoft.DataLakeAnalytics%2Faccounts%2Fadlpm/jobId/814e10ca-2e56-4814-8022-5632e19b561c";
            var portal_uri = AdlClient.Models.JobAzurePortalUri.Parse(s);

            Assert.IsNotNull(portal_uri);
            Assert.AreEqual("ace74b35-b0de-428b-a1d9-55459d7a6e30",portal_uri.SubscriptionId);
            Assert.AreEqual("adlpminsights", portal_uri.ResourceGroup);
            Assert.AreEqual("adlpm", portal_uri.Account);
            var expected_guid = System.Guid.Parse("814e10ca-2e56-4814-8022-5632e19b561c");
            Assert.AreEqual(expected_guid, portal_uri.JobId);
        }

        [TestMethod]
        public void ParseJobLink()
        {
            string s =
                "https://adlpm.azuredatalakeanalytics.net/jobs/814e10ca-2e56-4814-8022-5632e19b561c?api-version=2016-11-01";
            var job_uri = AdlClient.Models.JobUri.Parse(s);

            Assert.IsNotNull(job_uri);
            Assert.AreEqual("adlpm", job_uri.Account);
            var expected_guid = System.Guid.Parse("814e10ca-2e56-4814-8022-5632e19b561c");
            Assert.AreEqual(expected_guid, job_uri.JobId);
        }

    }
}
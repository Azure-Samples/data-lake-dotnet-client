namespace AdlClient.Models
{
    public class AnalyticsAccountPortalUri
    {
        // https://portal.azure.com/#resource/subscriptions/ace74b35-b0de-428b-a1d9-55459d7a6e30/resourcegroups/adlpminsights/providers/Microsoft.DataLakeAnalytics/accounts/adlpm/overview

        public readonly string SubscriptionId;
        public readonly string ResourceGroup;
        public readonly string Account;

        public AnalyticsAccountPortalUri(string subid, string rg, string account)
        {
            this.SubscriptionId = subid;
            this.ResourceGroup = rg;
            this.Account = account;
        }

        public AnalyticsAccountPortalUri(AnalyticsAccountRef job_ref)
        {
            this.SubscriptionId = job_ref.SubscriptionId;
            this.ResourceGroup = job_ref.ResourceGroup;
            this.Account = job_ref.Name;
        }

        public override string ToString()
        {
            string uri =
                string.Format(
                    "https://portal.azure.com/#resource/subscriptions/{0}/resourcegroups/{1}/providers/Microsoft.DataLakeAnalytics/accounts/{2}/jobId/{3}/overview",
                    this.SubscriptionId, this.ResourceGroup, this.Account);
            return uri;
        }
    }
}
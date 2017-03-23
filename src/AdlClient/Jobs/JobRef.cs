using System;

namespace AdlClient.Jobs
{
    public class JobRef
    {
        public readonly AnalyticsAccountRef Account;
        public readonly Guid Id;

        public JobRef( Guid id, AnalyticsAccountRef account)
        {
            this.Id = id;
            this.Account = account;
        }

        public string GetUri()
        {
            string uri = string.Format(
                "https://{0}.azuredatalakeanalytics.net/jobs/{1}?api-version=2015-10-01-preview", this.Account.Name,
                this.Id);
            return uri;
        }

        public string GetAzurePortalLink()
        {
            string uri =
                string.Format(
                    "https://portal.azure.com/#blade/Microsoft_Azure_DataLakeAnalytics/SqlIpJobDetailsBlade/accountId/%2Fsubscriptions%2F{0}%2FresourceGroups%2F{1}%2Fproviders%2FMicrosoft.DataLakeAnalytics%2Faccounts%2F{2}/jobId/{3}",
                    this.Account.SubscriptionId, this.Account.ResourceGroup, this.Account.Name, this.Id);
            return uri;
        }
    }
}
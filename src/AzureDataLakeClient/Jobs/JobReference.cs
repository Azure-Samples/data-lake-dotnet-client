using System;

namespace AdlClient.Jobs
{
    public class JobReference
    {
        public readonly AnalyticsAccount Account;
        public readonly Guid Id;

        public JobReference( Guid id, AnalyticsAccount account)
        {
            this.Id = id;
            this.Account = account;
        }

        public string GetUri()
        {
            string uri = $"https://{this.Account.Name}.azuredatalakeanalytics.net/jobs/{this.Id}?api-version=2015-10-01-preview";
            return uri;
        }

        public string GetAzurePortalLink()
        {
            string uri = $"https://portal.azure.com/#blade/Microsoft_Azure_DataLakeAnalytics/SqlIpJobDetailsBlade/accountId/%2Fsubscriptions%2F{this.Account.Subscription.Id}%2FresourceGroups%2F{this.Account.ResourceGroup.Name}%2Fproviders%2FMicrosoft.DataLakeAnalytics%2Faccounts%2F{this.Account.Name}/jobId/{this.Id}";
            return uri;
        }
    }
}
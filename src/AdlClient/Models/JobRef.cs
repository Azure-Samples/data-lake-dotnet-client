using System;

namespace AdlClient.Models
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

        public JobUri GetUri()
        {
            return new JobUri(this.Account.Name, this.Id);
        }

        public JobAzurePortalUri GetAzurePortalLink()
        {
            return new JobAzurePortalUri(this.Account.SubscriptionId,this.Account.ResourceGroup,this.Account.Name, this.Id);
        }
    }
}
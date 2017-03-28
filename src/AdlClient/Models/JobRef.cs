using System;

namespace AdlClient.Models
{
    public class JobRef
    {
        // JobRef uniquely identifies a job accross all subscriptions, accounts, resourcegroups

        public readonly AnalyticsAccountRef Account;
        public readonly Guid Id;

        public JobRef( Guid id, AnalyticsAccountRef account)
        {
            this.Id = id;
            this.Account = account;
        }
    }
}
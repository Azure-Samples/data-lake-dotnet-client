using System;

namespace AdlClient.Models
{
    public class JobRef
    {
        // JobRef uniquely identifies a job accross all subscriptions, accounts, resourcegroups

        public readonly string SubscriptionId;
        public readonly string ResourceGroup;
        public readonly string Account;
        public readonly Guid JobId;

        public JobRef(string subid, string rg, string account, Guid id)
        {
            this.SubscriptionId = subid;
            this.ResourceGroup = rg;
            this.Account = account;
            this.JobId = id;
        }
    }
}
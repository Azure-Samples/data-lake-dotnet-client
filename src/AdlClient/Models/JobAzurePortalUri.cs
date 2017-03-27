namespace AdlClient.Models
{
    public class JobAzurePortalUri
    {
        public readonly string SubscriptionId;
        public readonly string ResourceGroup;
        public readonly string Account;
        public readonly System.Guid Id;

        public JobAzurePortalUri(string subid, string rg, string account, System.Guid id)
        {
            this.SubscriptionId = subid;
            this.ResourceGroup = rg;
            this.Account = account;
            this.Id = id;
        }

        public JobAzurePortalUri(JobRef job_ref)
        {
            this.SubscriptionId = job_ref.Account.SubscriptionId;
            this.ResourceGroup = job_ref.Account.ResourceGroup;
            this.Account = job_ref.Account.Name;
            this.Id = job_ref.Id;
        }

        public static JobAzurePortalUri Parse(string s)
        {
            var uri = new System.Uri(s);

            var adl_portal_authority = "portal.azure.com";
            if (uri.Authority.ToLower() != adl_portal_authority)
            {
                string msg = string.Format("Malformed job portal uri: Uri.Authority must be with \"{0}\"", adl_portal_authority);
                throw new System.ArgumentException(msg);
            }

            string fragment = uri.Fragment;

            string fragment_prefix = "#blade/Microsoft_Azure_DataLakeAnalytics/SqlIpJobDetailsBlade/accountId/";

            if (!fragment.StartsWith(fragment_prefix))
            {
                throw new System.ArgumentException("invalid uri fragment");
            }

            var fragment_remainder = fragment.Substring(fragment_prefix.Length);
            fragment_remainder = System.Uri.UnescapeDataString(fragment_remainder);

            var fragment_tokens = fragment_remainder.Split('/');

            // Basic fragment validation

            if (fragment_tokens.Length < 11)
            {
                throw new System.ArgumentException("Malformed job portal uri: has Fragment has less than 11 parts");
            }

            if (fragment_tokens[0] != "")
            {
                throw new System.ArgumentException("Malformed job portal uri: first token should be empty");
            }

            // Subscription
            if (fragment_tokens[1] != "subscriptions")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"subscriptions\" in Fragment at position 1");
            }
            var subid = fragment_tokens[2];

            // Resource Group
            if (fragment_tokens[3] != "resourcegroups")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"resourcegroups\" in Fragment at position 3");
            }

            // Provider
            var rg = fragment_tokens[4];
            if (fragment_tokens[5] != "providers")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"providers\" in Fragment at position 5");
            }

            if (fragment_tokens[6] != "Microsoft.DataLakeAnalytics")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"Microsoft.DataLakeAnalytics\" in Fragment at position 6");
            }

            // ADLA Account
            if (fragment_tokens[7] != "accounts")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"accounts\" in Fragment at position 7");
            }

            var account_name = fragment_tokens[8];

            // JobId
            if (fragment_tokens[9] != "jobId")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"jobid\" in Fragment at position 9");
            }

            var jobid_str = fragment_tokens[10];
            var jobid = System.Guid.Parse(jobid_str);

            // Final URI
            var job_portal_uri = new JobAzurePortalUri(subid, rg, account_name, jobid);
            return job_portal_uri;
        }

        public override string ToString()
        {
            string uri =
                string.Format(
                    "https://portal.azure.com/#blade/Microsoft_Azure_DataLakeAnalytics/SqlIpJobDetailsBlade/accountId/%2Fsubscriptions%2F{0}%2FresourceGroups%2F{1}%2Fproviders%2FMicrosoft.DataLakeAnalytics%2Faccounts%2F{2}/jobId/{3}",
                    this.SubscriptionId, this.ResourceGroup, this.Account, this.Id);
            return uri;
        }

        public JobRef GetJobRef()
        {
            var account = new AnalyticsAccountRef(this.SubscriptionId,this.ResourceGroup,this.Account);
            return new JobRef(this.Id, account);
        }
    }
}
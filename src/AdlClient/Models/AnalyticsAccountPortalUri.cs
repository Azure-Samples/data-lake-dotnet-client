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

        public static AnalyticsAccountPortalUri Parse(string s)
        {
            var uri = new System.Uri(s);

            var adl_portal_authority = "portal.azure.com";
            if (uri.Authority.ToLower() != adl_portal_authority)
            {
                string msg = string.Format("Malformed job portal uri: Uri.Authority must be with \"{0}\"", adl_portal_authority);
                throw new System.ArgumentException(msg);
            }

            string fragment = uri.Fragment;

            string fragment_prefix = "#resource/";

            if (!fragment.StartsWith(fragment_prefix))
            {
                throw new System.ArgumentException("invalid uri fragment");
            }

            var fragment_remainder = fragment.Substring(fragment_prefix.Length);
            fragment_remainder = System.Uri.UnescapeDataString(fragment_remainder);

            var fragment_tokens = fragment_remainder.Split('/');

            // Basic fragment validation

            if (fragment_tokens.Length < 9)
            {
                //throw new System.ArgumentException("Malformed job portal uri: has Fragment has less than 11 parts");
            }

            // Subscription
            if (fragment_tokens[0] != "subscriptions")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"subscriptions\" in Fragment at position 0");
            }
            var subid = fragment_tokens[1];

            // Resource Group
            if (fragment_tokens[2] != "resourcegroups")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"resourcegroups\" in Fragment at position 2");
            }
            var rg = fragment_tokens[3];

            // Provider
            if (fragment_tokens[4] != "providers")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"providers\" in Fragment at position 4");
            }

            if (fragment_tokens[5] != "Microsoft.DataLakeAnalytics")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"Microsoft.DataLakeAnalytics\" in Fragment at position 5");
            }

            // ADLA Account
            if (fragment_tokens[6] != "accounts")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"accounts\" in Fragment at position 6");
            }

            var account_name = fragment_tokens[7];

            // Final URI
            var account_portal_uri = new AnalyticsAccountPortalUri(subid, rg, account_name);
            return account_portal_uri;
        }
    }
}
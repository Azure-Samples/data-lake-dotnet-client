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

        public static JobRef Parse(string s)
        {

            var id = System.Guid.Empty;
            var account = new AdlClient.Models.AnalyticsAccountRef("sub","rg","jn");
            var jr = new JobRef(id,account);

            var u = new System.Uri(s);
            string t = "#blade/Microsoft_Azure_DataLakeAnalytics/SqlIpJobDetailsBlade/accountId/";
            if (u.Authority.ToLower() == "portal.azure.com")
            {
                if (u.Fragment.StartsWith(t))
                {
                    var ss = u.Fragment.Substring(t.Length);

                    ss = System.Uri.UnescapeDataString(ss);

                    var tokens = ss.Split('/');

                    var subid = tokens[2];
                    var rg = tokens[4];
                    var account_name = tokens[8];
                    var jobid_st = tokens[10];
                    account = new AdlClient.Models.AnalyticsAccountRef(subid, rg, account_name);

                    var jobid = System.Guid.Parse(jobid_st);
                    jr = new JobRef(jobid, account);
                    return jr;
                }
                throw new System.ArgumentException("invalid uri fragment");
            }
            else if (u.Authority.ToLower().EndsWith(".azuredatalakeanalytics.net"))
            {
                //                "https://adlpm.azuredatalakeanalytics.net/jobs/814e10ca-2e56-4814-8022-5632e19b561c?api-version=2016-11-01";
                string ul = u.Authority.ToLower();
                var tokens = ul.Split('.');

                string account_name = tokens[0];

                string rg = null;

                string tt = u.LocalPath;
                var tokens2 = tt.Split('/');

                var jobid_st = tokens2[2];
                account = new AdlClient.Models.AnalyticsAccountRef(null, rg, account_name);

                var jobid = System.Guid.Parse(jobid_st);
                jr = new JobRef(jobid, account);
                return jr;
            }

            throw new System.ArgumentException("invalid uri authority");
        }
    }
}
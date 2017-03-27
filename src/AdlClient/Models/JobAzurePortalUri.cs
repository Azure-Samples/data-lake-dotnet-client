namespace AdlClient.Models
{
    public class JobAzurePortalUri
    {
        public string SubscriptionId;
        public string ResourceGroup;
        public string Account;
        public readonly System.Guid Id;

        public JobAzurePortalUri(string subid, string rg, string account, System.Guid id)
        {
            this.SubscriptionId = subid;
            this.ResourceGroup = rg;
            this.Account = account;
            this.Id = id;
        }


        public static JobAzurePortalUri Parse(string s)
        {

            var id = System.Guid.Empty;
            var account = new AdlClient.Models.AnalyticsAccountRef("sub", "rg", "jn");
            var jr = new JobRef(id, account);

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

                    var jobid = System.Guid.Parse(jobid_st);
                    var jobportaluri = new JobAzurePortalUri(subid, rg, account_name, jobid);
                    return jobportaluri;
                }
                throw new System.ArgumentException("invalid uri fragment");
            }
            throw new System.ArgumentException("invalid uri authority");
        }

        public override string ToString()
        {
            string uri =
                string.Format(
                    "https://portal.azure.com/#blade/Microsoft_Azure_DataLakeAnalytics/SqlIpJobDetailsBlade/accountId/%2Fsubscriptions%2F{0}%2FresourceGroups%2F{1}%2Fproviders%2FMicrosoft.DataLakeAnalytics%2Faccounts%2F{2}/jobId/{3}",
                    this.SubscriptionId, this.ResourceGroup, this.Account, this.Id);
            return uri;
        }
    }
}
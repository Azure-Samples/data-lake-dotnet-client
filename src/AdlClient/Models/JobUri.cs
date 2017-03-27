namespace AdlClient.Models
{
    public class JobUri
    {
        public readonly string Account;
        public readonly System.Guid Id;

        public JobUri(string account, System.Guid id)
        {
            this.Account = account;
            this.Id = id;
        }

        public static JobUri Parse(string s)
        {
            var u = new System.Uri(s);
            var authority_lc = u.Authority.ToLower();
            if (authority_lc.EndsWith(".azuredatalakeanalytics.net"))
            {
                // Example: "https://adlpm.azuredatalakeanalytics.net/jobs/814e10ca-2e56-4814-8022-5632e19b561c?api-version=2016-11-01";
                var tokens = authority_lc.Split('.');

                string account_name = tokens[0];


                string tt = u.LocalPath;
                var tokens2 = tt.Split('/');

                var jobid_st = tokens2[2];

                var jobid = System.Guid.Parse(jobid_st);
                var joblink = new JobUri(account_name, jobid);
                return joblink;
            }
            throw new System.ArgumentException("invalid uri authority");
        }


        public override string ToString()
        {
            string uri = string.Format(
                "https://{0}.azuredatalakeanalytics.net/jobs/{1}?api-version=2015-10-01-preview", this.Account,
                this.Id);
            return uri;
        }
    }
}
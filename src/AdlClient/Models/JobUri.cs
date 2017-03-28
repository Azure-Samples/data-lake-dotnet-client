namespace AdlClient.Models
{
    public class JobUri
    {
        public readonly string Account;
        public readonly System.Guid JobId;

        public JobUri(string account, System.Guid jobId)
        {
            this.Account = account;
            this.JobId = jobId;
        }

        public JobUri(JobRef job_ref)
        {
            this.Account = job_ref.Account.Name;
            this.JobId = job_ref.Id;
        }

        public static JobUri Parse(string s)
        {
            // Example: "https://adlpm.azuredatalakeanalytics.net/jobs/814e10ca-2e56-4814-8022-5632e19b561c?api-version=2016-11-01";

            var uri = new System.Uri(s);
            var authority_lc = uri.Authority.ToLower();

            var adl_authority_suffix = ".azuredatalakeanalytics.net";
            if (!authority_lc.EndsWith(adl_authority_suffix))
            {
                string msg = string.Format("Malformed job uri: Uri.Authority should end with \"{0}\"",
                    adl_authority_suffix);
                throw new System.ArgumentException(msg);
            }

            // Find the account name
            var authority_tokens = authority_lc.Split('.');
            string account_name = authority_tokens[0];

            // Break up the path to find the job id
            var localpath_tokens = uri.LocalPath.Split('/');

            if (localpath_tokens.Length < 3)
            {
                throw new System.ArgumentException("Malformed job uri: has LocalPath has less than 3 parts");
            }

            if (localpath_tokens[0] != "")
            {
                throw new System.ArgumentException("Malformed job uri: first token should be empty");
            }

            if (localpath_tokens[1] != "jobs")
            {
                throw new System.ArgumentException("Malformed job uri: missing \"jobs\" in LocalPath");
            }

            var jobid_str = localpath_tokens[2];
            var jobid = System.Guid.Parse(jobid_str);
            var joburi = new JobUri(account_name, jobid);
            return joburi;
        }


        public override string ToString()
        {
            string uri = string.Format(
                "https://{0}.azuredatalakeanalytics.net/jobs/{1}?api-version=2015-10-01-preview", this.Account,
                this.JobId);
            return uri;
        }
    }
}
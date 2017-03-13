namespace AzureDataLakeClient.Extensions
{
    public class AnalyticsExtensions
    {
        public string GetJobUrl(Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation job, string account)
        {
            string s = string.Format("https://{0}.azuredatalakeanalytics.net/jobs/{1}", account,
                job.JobId.Value.ToString());

            return s;
        }

        public string GetJobPortalUrl(Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation job, string account, string sub_id, string resource_group)
        {
            var bladename = "SqlIpJobDetailsBlade";
            var subscription = sub_id;
            var provider = "Microsoft.DataLakeAnalytics";
            var prefix = "Microsoft_Azure_DataLakeAnalytics";
            string s = "https://portal.azure.com/?feature.customportal=false#blade/" + prefix + "/"
                       + bladename+ "/accountId/" + "%2Fsubscriptions%2F" +
                       subscription + "3%2FresourceGroups%2F" +
                       resource_group + "%2Fproviders%2F" + provider + "%2Faccounts%2F" + account +
                       "/jobId/" + job.JobId.ToString() ;

            return s;
        }


    }
}
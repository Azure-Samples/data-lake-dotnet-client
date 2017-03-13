using System.Collections.Generic;
using AzureDataLakeClient.Jobs;
using Microsoft.Azure.Management.DataLake.Analytics;

namespace AzureDataLakeClient.Rest
{
    public class AnalyticsJobsRestWrapper
    {
        private Microsoft.Azure.Management.DataLake.Analytics.DataLakeAnalyticsJobManagementClient _client;
        private Microsoft.Rest.ServiceClientCredentials _creds;

        public AnalyticsJobsRestWrapper(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._creds = creds;
            this._client = new Microsoft.Azure.Management.DataLake.Analytics.DataLakeAnalyticsJobManagementClient(this._creds);
        }

        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation JobGet(AnalyticsAccount analyticsaccount, System.Guid jobid)
        {
            var job = this._client.Job.Get(analyticsaccount.Name, jobid);
            return job;
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation> JobList(AnalyticsAccount account,
            Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation> odata_query, int top)
        {
            // Other parameters
            string opt_select = null;
            bool? opt_count = null;

            int item_count = 0;
            var page = this._client.Job.List(account.Name, odata_query, opt_select, opt_count);
            foreach (
                var job in
                RestUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation>(page,
                    p => this._client.Job.ListNext(p.NextPageLink)))
            {
                yield return job;
                item_count++;

                if ((top > 0) && (item_count >= top))
                {
                    break;
                }
            }

        }

        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation JobCreate(AnalyticsAccount account
            , SubmitJobOptions options)
        {
            var parameters = CreateNewJobProperties(options);
            var job_info = this._client.Job.Create(account.Name, options.JobID, parameters);

            return job_info;

        }

        private static Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation CreateNewJobProperties(SubmitJobOptions options)
        {
            var jobprops = new Microsoft.Azure.Management.DataLake.Analytics.Models.USqlJobProperties();
            jobprops.Script = options.ScriptText;

            var jobType = Microsoft.Azure.Management.DataLake.Analytics.Models.JobType.USql;
            int priority = 1000; // 1000 is default priority for a new job
            int dop = 1;

            var parameters = new Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation(
                name: options.JobName,
                type: jobType,
                properties: jobprops,
                priority: priority,
                degreeOfParallelism: dop,
                jobId: options.JobID);
            return parameters;
        }

        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobStatistics GetStatistics(AnalyticsAccount account, System.Guid jobid)
        {
            var stats = this._client.Job.GetStatistics(account.Name, jobid);
            return stats;
        }

        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobDataPath GetDebugDataPath(AnalyticsAccount account, System.Guid jobid)
        {
            var jobdatapath = this._client.Job.GetDebugDataPath(account.Name, jobid);
            return jobdatapath;
        }
    }
}
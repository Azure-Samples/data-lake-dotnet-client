using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;

namespace AzureDataLakeClient.Analytics.Clients
{
    public class AnalyticsJobsRestClient
    {
        private Microsoft.Azure.Management.DataLake.Analytics.DataLakeAnalyticsJobManagementClient _client;
        private Microsoft.Rest.ServiceClientCredentials _creds;

        public AnalyticsJobsRestClient(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._creds = creds;
            this._client = new Microsoft.Azure.Management.DataLake.Analytics.DataLakeAnalyticsJobManagementClient(this._creds);
        }

        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation JobGet(AnalyticsAccountUri analyticsaccount, System.Guid jobid)
        {
            var job = this._client.Job.Get(analyticsaccount.Name, jobid);
            return job;
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation> JobList(AnalyticsAccountUri account,
            Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation> odata_query, int top)
        {
            // Other parameters
            string opt_select = null;
            bool? opt_count = null;

            int item_count = 0;
            var page = this._client.Job.List(account.Name, odata_query, opt_select, opt_count);
            foreach (
                var job in
                RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation>(page,
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

        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation JobCreate(AnalyticsAccountUri account
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
            int priority = 1;
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

        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobStatistics GetStatistics(AnalyticsAccountUri account, System.Guid jobid)
        {
            var stats = this._client.Job.GetStatistics(account.Name, jobid);
            return stats;
        }

        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobDataPath GetDebugDataPath(AnalyticsAccountUri account, System.Guid jobid)
        {
            var jobdatapath = this._client.Job.GetDebugDataPath(account.Name, jobid);
            return jobdatapath;
        }
    }
}
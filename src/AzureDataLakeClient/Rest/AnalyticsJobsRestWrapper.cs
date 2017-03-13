using System.Collections.Generic;
using AzureDataLakeClient.Jobs;
using Microsoft.Azure.Management.DataLake.Analytics;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AzureDataLakeClient.Rest
{
    public class AnalyticsJobsRestWrapper
    {
        private MSADLA.DataLakeAnalyticsJobManagementClient _client;
        private Microsoft.Rest.ServiceClientCredentials _creds;

        public AnalyticsJobsRestWrapper(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._creds = creds;
            this._client = new MSADLA.DataLakeAnalyticsJobManagementClient(this._creds);
        }

        public MSADLA.Models.JobInformation JobGet(AnalyticsAccount analyticsaccount, System.Guid jobid)
        {
            var job = this._client.Job.Get(analyticsaccount.Name, jobid);
            return job;
        }

        public IEnumerable<MSADLA.Models.JobInformation> JobList(AnalyticsAccount account,
            Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.JobInformation> odata_query, int top)
        {
            // Other parameters
            string opt_select = null;
            bool? opt_count = null;

            int item_count = 0;
            var page = this._client.Job.List(account.Name, odata_query, opt_select, opt_count);
            foreach (
                var job in
                RestUtil.EnumItemsInPages<MSADLA.Models.JobInformation>(page,
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

        public JobInfo JobCreate(AnalyticsAccount account, SubmitJobOptions options)
        {
            var job_props = options.CreateNewJobProperties();
            var job_info = this._client.Job.Create(account.Name, options.JobID, job_props);
            var j = new JobInfo(job_info, account);
            return j;
        }

        public MSADLA.Models.JobStatistics GetStatistics(AnalyticsAccount account, System.Guid jobid)
        {
            var stats = this._client.Job.GetStatistics(account.Name, jobid);
            return stats;
        }

        public MSADLA.Models.JobDataPath GetDebugDataPath(AnalyticsAccount account, System.Guid jobid)
        {
            var jobdatapath = this._client.Job.GetDebugDataPath(account.Name, jobid);
            return jobdatapath;
        }
    }
}
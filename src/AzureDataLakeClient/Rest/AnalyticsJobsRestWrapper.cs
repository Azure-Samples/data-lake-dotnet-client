using System.Collections.Generic;
using AdlClient.Jobs;
using Microsoft.Azure.Management.DataLake.Analytics;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Rest
{
    public class AnalyticsJobsRestWrapper
    {
        public MSADLA.DataLakeAnalyticsJobManagementClient RestClient;

        public AnalyticsJobsRestWrapper(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.RestClient = new MSADLA.DataLakeAnalyticsJobManagementClient(creds);
        }

        public MSADLA.Models.JobInformation JobGet(AnalyticsAccount analyticsaccount, System.Guid jobid)
        {
            var job = this.RestClient.Job.Get(analyticsaccount.Name, jobid);
            return job;
        }

        public void JobCancel(AnalyticsAccount analyticsaccount, System.Guid jobid)
        {
            this.RestClient.Job.Cancel(analyticsaccount.Name, jobid);
        }

        public MSADLA.Models.JobInformation JobBuild(AnalyticsAccount analyticsaccount, MSADLA.Models.JobInformation parameters)
        {
            var j = this.RestClient.Job.Build(analyticsaccount.Name, parameters);
            return j;
        }

        public bool JobExists(AnalyticsAccount analyticsaccount, System.Guid jobid)
        {
            return this.RestClient.Job.Exists(analyticsaccount.Name, jobid);
        }

        public IEnumerable<MSADLA.Models.JobInformation> JobList(AnalyticsAccount account,
            Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.JobInformation> odata_query, int top)
        {
            // Other parameters
            string opt_select = null;
            bool? opt_count = null;

            int item_count = 0;
            var page = this.RestClient.Job.List(account.Name, odata_query, opt_select, opt_count);
            foreach (
                var job in
                RestUtil.EnumItemsInPages<MSADLA.Models.JobInformation>(page,
                    p => this.RestClient.Job.ListNext(p.NextPageLink)))
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
            var job_info = this.RestClient.Job.Create(account.Name, options.JobID, job_props);
            var j = new JobInfo(job_info, account);
            return j;
        }

        public MSADLA.Models.JobStatistics GetStatistics(AnalyticsAccount account, System.Guid jobid)
        {
            var stats = this.RestClient.Job.GetStatistics(account.Name, jobid);
            return stats;
        }

        public MSADLA.Models.JobDataPath GetDebugDataPath(AnalyticsAccount account, System.Guid jobid)
        {
            var jobdatapath = this.RestClient.Job.GetDebugDataPath(account.Name, jobid);
            return jobdatapath;
        }
    }
}
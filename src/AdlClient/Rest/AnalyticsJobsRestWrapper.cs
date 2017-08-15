using System;
using System.Collections.Generic;
using AdlClient.Models;
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

        public MSADLA.Models.JobInformation JobGet(AnalyticsAccountRef analyticsaccount, System.Guid jobid)
        {
            var job = this.RestClient.Job.Get(analyticsaccount.Name, jobid);
            return job;
        }

        public void JobCancel(AnalyticsAccountRef analyticsaccount, System.Guid jobid)
        {
            this.RestClient.Job.Cancel(analyticsaccount.Name, jobid);
        }

        public bool JobExists(AnalyticsAccountRef analyticsaccount, System.Guid jobid)
        {
            return this.RestClient.Job.Exists(analyticsaccount.Name, jobid);
        }

        public IEnumerable<MSADLA.Models.JobInformation> JobList(AnalyticsAccountRef account,
            Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.JobInformation> odata_query, int top)
        {
            // Other parameters
            string opt_select = null;
            bool? opt_count = null;

            int item_count = 0;
            var page = this.RestClient.Job.List(account.Name, odata_query, opt_select, opt_count);
            foreach (
                var job in
                RestUtil.EnumItemsInPages(page, p => this.RestClient.Job.ListNext(p.NextPageLink)))
            {
                yield return job;
                item_count++;

                if ((top > 0) && (item_count >= top))
                {
                    break;
                }
            }
        }

        public IEnumerable<MSADLA.Models.JobRecurrenceInformation> JobRecurrenceList(AnalyticsAccountRef account, DateTimeOffset? start, DateTimeOffset? end, int top)
        {
            int item_count = 0;
            var page = this.RestClient.Recurrence.List(account.Name, start, end);
            foreach (
                var job in
                RestUtil.EnumItemsInPages(page, p => this.RestClient.Recurrence.ListNext(p.NextPageLink)))
            {
                yield return job;
                item_count++;

                if ((top > 0) && (item_count >= top))
                {
                    break;
                }
            }
        }

        public IEnumerable<MSADLA.Models.JobPipelineInformation> JobPipelineInformationList(AnalyticsAccountRef account, DateTimeOffset? start, DateTimeOffset? end, int top)
        {
            int item_count = 0;
            var page = this.RestClient.Pipeline.List(account.Name, start, end);
            foreach (
                var job in
                RestUtil.EnumItemsInPages(page, p => this.RestClient.Pipeline.ListNext(p.NextPageLink)))
            {
                yield return job;
                item_count++;

                if ((top > 0) && (item_count >= top))
                {
                    break;
                }
            }
        }

        public JobInfo JobCreate(AnalyticsAccountRef account, JobSubmitParameters parameters)
        {
            var job_props = parameters.ToJobInformationObject();
            var job_info = this.RestClient.Job.Create(account.Name, parameters.JobId, job_props);
            var j = new JobInfo(job_info, account);
            return j;
        }

        public JobInfo JobBuild(AnalyticsAccountRef account, JobSubmitParameters parameters)
        {
            var job_props = parameters.ToJobInformationObject();
            var job_info = this.RestClient.Job.Build(account.Name, job_props);
            var j = new JobInfo(job_info, account);
            return j;
        }


        public MSADLA.Models.JobStatistics GetStatistics(AnalyticsAccountRef account, System.Guid jobid)
        {
            var stats = this.RestClient.Job.GetStatistics(account.Name, jobid);
            return stats;
        }

        public MSADLA.Models.JobDataPath GetDebugDataPath(AnalyticsAccountRef account, System.Guid jobid)
        {
            var jobdatapath = this.RestClient.Job.GetDebugDataPath(account.Name, jobid);
            return jobdatapath;
        }
    }
}
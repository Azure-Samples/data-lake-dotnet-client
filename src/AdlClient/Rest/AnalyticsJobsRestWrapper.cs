using System;
using System.Collections.Generic;
using AdlClient.Models;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Rest.Azure;
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

            var pageiter = new PagedIterator<MSADLA.Models.JobInformation>();
            pageiter.GetFirstPage = () => this.RestClient.Job.List(account.Name, odata_query, opt_select, opt_count);
            pageiter.GetNextPage = p => this.RestClient.Job.ListNext(p.NextPageLink);

            var jobs = pageiter.EnumerateItems(top);

            return jobs;
        }

        public IEnumerable<MSADLA.Models.JobRecurrenceInformation> JobRecurrenceList(AnalyticsAccountRef account, DateTimeOffset? start, DateTimeOffset? end, int top)
        {

            var pageiter = new PagedIterator<MSADLA.Models.JobRecurrenceInformation>();
            pageiter.GetFirstPage = () => this.RestClient.Recurrence.List(account.Name, start, end);
            pageiter.GetNextPage = p => this.RestClient.Recurrence.ListNext(p.NextPageLink);

            var items = pageiter.EnumerateItems(top);

            return items;
        }

        public IEnumerable<MSADLA.Models.JobPipelineInformation> JobPipelineInformationList(AnalyticsAccountRef account, DateTimeOffset? start, DateTimeOffset? end, int top)
        {
            var pageiter = new PagedIterator<MSADLA.Models.JobPipelineInformation>();
            pageiter.GetFirstPage = () => this.RestClient.Pipeline.List(account.Name, start, end);
            pageiter.GetNextPage = p => this.RestClient.Pipeline.ListNext(p.NextPageLink);

            var items = pageiter.EnumerateItems(top);

            return items;
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
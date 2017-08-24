using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using AdlClient.Models;
using Microsoft.Azure.Management.DataLake.Analytics;
using MSADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Commands
{
    public class JobCommands
    {
        internal readonly AnalyticsAccountRef Account;
        internal readonly AnalyticsRestClients RestClients;

        internal JobCommands(AnalyticsAccountRef a, AnalyticsRestClients clients)
        {
            this.Account = a;
            this.RestClients = clients;
        }

        public void CancelJob(System.Guid jobid)
        {
            this.RestClients.JobsClient.Job.Cancel(this.Account.Name, jobid);
        }

        public bool JobExists(System.Guid jobid)
        {
            return this.RestClients.JobsClient.Job.Exists(this.Account.Name, jobid);
        }

        public JobDetails GetJobDetails(System.Guid jobid, bool extendedInfo)
        {

            var job = this.RestClients.JobsClient.Job.Get(this.Account.Name, jobid);

            var jobinfo = new JobInfo(job, this.Account);

            var jobdetails = new JobDetails();
            jobdetails.JobInfo = jobinfo;

            jobdetails.StateAuditRecords = job.StateAuditRecords;
            jobdetails.Properties = job.Properties;
            jobdetails.ErrorMessage = job.ErrorMessage;
            jobdetails.LogFilePatterns = job.LogFilePatterns;

            if (extendedInfo)
            {
                jobdetails.JobDetailsExtended = new JobDetailsExtended();


                jobdetails.JobDetailsExtended.Statistics = this.RestClients.JobsClient.Job.GetStatistics(this.Account.Name, jobid);

                // jobdetails.JobDetailsExtended.DebugDataPath = this.clients._JobRest.GetDebugDataPath(this.account, jobid);
            }

            return jobdetails;
        }

        public IEnumerable<JobInfo> ListJobs(JobListingParameters parameters)
        {
            var odata_query = new Microsoft.Rest.Azure.OData.ODataQuery<MSADL.Analytics.Models.JobInformation>();

            odata_query.OrderBy = parameters.Sorting.CreateOrderByString();
            odata_query.Filter = parameters.Filter.ToFilterString();

            // enumerate the job objects
            // Other parameters
            string opt_select = null;
            bool? opt_count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.JobInformation>();
            pageiter.GetFirstPage = () => this.RestClients.JobsClient.Job.List(this.Account.Name, odata_query, opt_select, opt_count);
            pageiter.GetNextPage = p => this.RestClients.JobsClient.Job.ListNext(p.NextPageLink);

            var jobs = pageiter.EnumerateItems(parameters.Top);

            // convert them to the JobInfo
            var jobinfos = jobs.Select(j => new JobInfo(j, this.Account));

            return jobinfos;
        }

        public IEnumerable<MSADL.Analytics.Models.JobPipelineInformation> ListJobPipelines(JobPipelineListingParameters parameters)
        {
            var pageiter = new Rest.PagedIterator<MSADLA.Models.JobPipelineInformation>();
            pageiter.GetFirstPage = () => this.RestClients.JobsClient.Pipeline.List(this.Account.Name, parameters.DateRange.LowerBound, parameters.DateRange.UpperBound);
            pageiter.GetNextPage = p => this.RestClients.JobsClient.Pipeline.ListNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);

            return items;
        }

        public IEnumerable<MSADL.Analytics.Models.JobRecurrenceInformation> ListJobRecurrences(JobReccurenceListingParameters parameters)
        {

            var pageiter = new Rest.PagedIterator<MSADLA.Models.JobRecurrenceInformation>();
            pageiter.GetFirstPage = () => this.RestClients.JobsClient.Recurrence.List(this.Account.Name, parameters.DateRange.LowerBound, parameters.DateRange.UpperBound);
            pageiter.GetNextPage = p => this.RestClients.JobsClient.Recurrence.ListNext(p.NextPageLink);

            int top = 0;
            var recurrences = pageiter.EnumerateItems(top);
            return recurrences;
        }

        public JobInfo CreateJob(CreateJobParameters parameters)
        {
            FixupCreateJobParameters(parameters);

            var job_props = parameters.ToJobInformationObject();
            var job_info = this.RestClients.JobsClient.Job.Create(this.Account.Name, parameters.JobId, job_props);
            var j = new JobInfo(job_info, this.Account);
            return j;
        }

        private static void FixupCreateJobParameters(CreateJobParameters parameters)
        {
            // If caller doesn't provide a guid, then create a new one
            if (parameters.JobId == default(System.Guid))
            {
                parameters.JobId = System.Guid.NewGuid();
            }

            // if caller doesn't provide a name, then create one automativally
            if (parameters.JobName == null)
            {
                // TODO: Handle the date part of the name nicely
                parameters.JobName = "USQL " + System.DateTimeOffset.Now.ToString();
            }
        }

        public JobInfo BuildJob(CreateJobParameters parameters)
        {
            FixupCreateJobParameters(parameters);


            var job_props = parameters.ToJobInformationObject();
            var job_info = this.RestClients.JobsClient.Job.Build(this.Account.Name, job_props);
            var j = new JobInfo(job_info, this.Account);
            return j;
        }

        public MSADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            return this.RestClients.JobsClient.Job.GetStatistics(this.Account.Name, jobid);
        }

        public MSADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            var jobdatapath = this.RestClients.JobsClient.Job.GetDebugDataPath(this.Account.Name, jobid);
            return jobdatapath;
        }
    }
}
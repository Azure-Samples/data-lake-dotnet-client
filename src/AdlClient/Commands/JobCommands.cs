using System.Collections.Generic;
using System.Linq;
using AdlClient.Jobs;
using MSADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Commands
{
    public class JobCommands
    {
        public static int ADLJobPageSize = 300; // The maximum page size for ADLA list is 300

        private readonly AnalyticsAccountRef account;
        private readonly AnalyticsRestClients clients;

        internal JobCommands(AnalyticsAccountRef a, AnalyticsRestClients clients)
        {
            this.account = a;
            this.clients = clients;
        }

        public JobDetails GetJobDetails(System.Guid jobid, bool extendedInfo)
        {
            var job = this.clients._JobRest.JobGet(this.account, jobid);

            var jobinfo = new JobInfo(job, this.account);

            var jobdetails = new JobDetails();
            jobdetails.JobInfo = jobinfo;

            jobdetails.StateAuditRecords = job.StateAuditRecords;
            jobdetails.Properties = job.Properties;
            jobdetails.ErrorMessage = job.ErrorMessage;
            jobdetails.LogFilePatterns = job.LogFilePatterns;

            if (extendedInfo)
            {
                jobdetails.JobDetailsExtended = new JobDetailsExtended();
                jobdetails.JobDetailsExtended.Statistics = this.clients._JobRest.GetStatistics(this.account, jobid);

                // jobdetails.JobDetailsExtended.DebugDataPath = this.clients._JobRest.GetDebugDataPath(this.account, jobid);
            }

            return jobdetails;
        }

        public IEnumerable<JobInfo> ListJobs(JobListingParameters parameters)
        {
            var odata_query = new Microsoft.Rest.Azure.OData.ODataQuery<MSADL.Analytics.Models.JobInformation>();

            // if users requests top, set the value appropriately relative to the page size
            if ((parameters.Top > 0) && (parameters.Top <= JobCommands.ADLJobPageSize))
            {
                odata_query.Top = parameters.Top;
            }

            odata_query.OrderBy = parameters.Sorting.CreateOrderByString();
            odata_query.Filter = parameters.Filter.ToFilterString();

            // enumerate the job objects
            var jobs = this.clients._JobRest.JobList(this.account, odata_query, parameters.Top);

            // convert them to the JobInfo
            var jobinfos = jobs.Select(j => new JobInfo(j, this.account));

            return jobinfos;
        }

        public JobInfo SubmitJob(JobSubmitParameters parameters)
        {
            FixupOptions(parameters);
            var job_info = this.clients._JobRest.JobCreate(this.account, parameters);
            return job_info;
        }

        private static void FixupOptions(JobSubmitParameters parameters)
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

        public JobInfo BuildJob(JobSubmitParameters parameters)
        {
            FixupOptions(parameters);
            var job_info = this.clients._JobRest.JobBuild(this.account, parameters);
            return job_info;
        }

        public MSADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            return this.clients._JobRest.GetStatistics(this.account, jobid);
        }

        public MSADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            return this.clients._JobRest.GetDebugDataPath(this.account, jobid);
        }



    }
}
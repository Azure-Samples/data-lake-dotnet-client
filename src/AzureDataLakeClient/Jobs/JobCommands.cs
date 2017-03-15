using System.Collections.Generic;
using System.Linq;
using MSADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Jobs
{
    public class JobCommands
    {
        public static int ADLJobPageSize = 300; // The maximum page size for ADLA list is 300

        private AnalyticsAccount account;
        private AnalyticsRestClients clients;

        internal JobCommands(AnalyticsAccount a, AnalyticsRestClients clients)
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

            if (extendedInfo)
            {
                jobdetails.ExtendedJobInfo = new ExtendedJobInfo();
                jobdetails.ExtendedJobInfo.Statistics = this.clients._JobRest.GetStatistics(this.account, jobid);

                jobdetails.ExtendedJobInfo.ErrorMessage = job.ErrorMessage;
                jobdetails.ExtendedJobInfo.LogFilePatterns = job.LogFilePatterns;

                // jobdetails.ExtendedJobInfo.DebugDataPath = this.clients._JobRest.GetDebugDataPath(this.account, jobid);
            }

            return jobdetails;
        }

        public IEnumerable<JobInfo> GetJobs(GetJobsOptions options)
        {
            var odata_query = new Microsoft.Rest.Azure.OData.ODataQuery<MSADL.Analytics.Models.JobInformation>();

            // if users requests top, set the value appropriately relative to the page size
            if ((options.Top > 0) && (options.Top <= JobCommands.ADLJobPageSize))
            {
                odata_query.Top = options.Top;
            }

            odata_query.OrderBy = options.Sorting.CreateOrderByString();
            odata_query.Filter = options.Filter.ToFilterString();

            // enumerate the job objects
            var jobs = this.clients._JobRest.JobList(this.account, odata_query, options.Top);

            // convert them to the JobInfo
            var jobinfos = jobs.Select(j => new JobInfo(j, this.account));

            return jobinfos;
        }

        public JobInfo SubmitJob(SubmitJobOptions options)
        {
            FixupOptions(options);
            var job_info = this.clients._JobRest.JobCreate(this.account, options);
            return job_info;
        }

        private static void FixupOptions(SubmitJobOptions options)
        {
            // If caller doesn't provide a guid, then create a new one
            if (options.JobId == default(System.Guid))
            {
                options.JobId = System.Guid.NewGuid();
            }

            // if caller doesn't provide a name, then create one automativally
            if (options.JobName == null)
            {
                // TODO: Handle the date part of the name nicely
                options.JobName = "USQL " + System.DateTimeOffset.Now.ToString();
            }
        }

        public JobInfo BuildJob(SubmitJobOptions options)
        {
            FixupOptions(options);
            var job_info = this.clients._JobRest.JobBuild(this.account, options);
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
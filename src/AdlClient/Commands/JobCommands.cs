using System.Collections.Generic;
using System.Linq;
using AdlClient.Models;
using MSADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Commands
{
    public class JobCommands
    {
        internal readonly AnalyticsAccountRef Account;
        internal readonly AnalyticsRestClients RestClients;

        public static int ADLJobPageSize = 300; // The maximum page size for ADLA list is 300

        internal JobCommands(AnalyticsAccountRef a, AnalyticsRestClients clients)
        {
            this.Account = a;
            this.RestClients = clients;
        }

        public JobDetails GetJobDetails(System.Guid jobid, bool extendedInfo)
        {
            var job = this.RestClients._JobRest.JobGet(this.Account, jobid);

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
                jobdetails.JobDetailsExtended.Statistics = this.RestClients._JobRest.GetStatistics(this.Account, jobid);

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
            var jobs = this.RestClients._JobRest.JobList(this.Account, odata_query, parameters.Top);

            // convert them to the JobInfo
            var jobinfos = jobs.Select(j => new JobInfo(j, this.Account));

            return jobinfos;
        }

        public JobInfo SubmitJob(JobSubmitParameters parameters)
        {
            FixupSubmitParameters(parameters);
            var job_info = this.RestClients._JobRest.JobCreate(this.Account, parameters);
            return job_info;
        }

        private static void FixupSubmitParameters(JobSubmitParameters parameters)
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
            FixupSubmitParameters(parameters);
            var job_info = this.RestClients._JobRest.JobBuild(this.Account, parameters);
            return job_info;
        }

        public MSADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            return this.RestClients._JobRest.GetStatistics(this.Account, jobid);
        }

        public MSADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            return this.RestClients._JobRest.GetDebugDataPath(this.Account, jobid);
        }



    }
}
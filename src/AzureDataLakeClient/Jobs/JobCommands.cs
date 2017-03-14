using System.Collections.Generic;
using MSADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Jobs
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

        public JobInfo GetJob(System.Guid jobid)
        {
            var job = this.clients._JobRest.JobGet(this.account, jobid);

            var j = new JobInfo(job, this.account);
            return j;
        }

        public JobInfo GetJobExtendedInfo(System.Guid jobid)
        {
            var job = this.clients._JobRest.JobGet(this.account, jobid);
            
            var jop_Info = new JobInfo(job, this.account);

            var statistics = this.clients._JobRest.GetStatistics(this.account, jobid);
            var debugpaths = clients._JobRest.GetDebugDataPath(this.account, jobid);

            jop_Info.ExtendedInfo.Statistics = statistics;
            jop_Info.ExtendedInfo.DebugDataPath = debugpaths;

            return jop_Info;
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

            var jobs = this.clients._JobRest.JobList(this.account, odata_query, options.Top);
            foreach (var job in jobs)
            {
                var j = new JobInfo(job, this.account);
                yield return j;
            }
        }

        public JobInfo SubmitJob(SubmitJobOptions options)
        {
            // If caller doesn't provide a guid, then create a new one
            if (options.JobID == default(System.Guid))
            {
                options.JobID = System.Guid.NewGuid();
            }

            // if caller doesn't provide a name, then create one automativally
            if (options.JobName == null)
            {
                // TODO: Handle the date part of the name nicely
                options.JobName = "USQL " + System.DateTimeOffset.Now.ToString();
            }

            var job_info = this.clients._JobRest.JobCreate(this.account, options);

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
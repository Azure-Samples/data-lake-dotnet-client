using System.Collections.Generic;
using AzureDataLakeClient.Rest;
using MSD_ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics.Jobs
{

    public class JobCommands
    {
        public static int ADLJobPageSize = 300; // The maximum page size for ADLA list is 300

        private AnalyticsJobsRestWrapper _adlaJobRestWrapper;
        private AnalyticsAccount account;

        public JobCommands(AnalyticsAccount a, AnalyticsJobsRestWrapper c)
        {
            this.account = a;
            this._adlaJobRestWrapper = c;
        }

        public MSD_ADL.Analytics.Models.JobInformation GetJob(System.Guid jobid)
        {
            var job = this._adlaJobRestWrapper.JobGet(this.account, jobid);
            return job;
        }

        public IEnumerable<JobInfo> GetJobs(GetJobsOptions options)
        {
            var odata_query = new Microsoft.Rest.Azure.OData.ODataQuery<MSD_ADL.Analytics.Models.JobInformation>();

            // if users requests top, set the value appropriately relative to the page size
            if ((options.Top > 0) && (options.Top <= JobCommands.ADLJobPageSize))
            {
                odata_query.Top = options.Top;
            }

            odata_query.OrderBy = options.Sorting.CreateOrderByString();
            odata_query.Filter = options.Filter.ToFilterString();


            var jobs = this._adlaJobRestWrapper.JobList(this.account, odata_query, options.Top);
            foreach (var job in jobs)
            {
                var j = new JobInfo(job, this.account);
                yield return j;
            }
        }

        public MSD_ADL.Analytics.Models.JobInformation SubmitJob(SubmitJobOptions options)
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

            var job_info = this._adlaJobRestWrapper.JobCreate(this.account, options);
            return job_info;
        }

        public MSD_ADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            return this._adlaJobRestWrapper.GetStatistics(this.account, jobid);
        }

        public MSD_ADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            return this._adlaJobRestWrapper.GetDebugDataPath(this.account, jobid);
        }



    }
}
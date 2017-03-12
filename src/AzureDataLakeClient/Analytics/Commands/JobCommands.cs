using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics.Commands
{
    public class JobCommands
    {
        public static int ADLJobPageSize = 300; // The maximum page size for ADLA list is 300

        private AnalyticsJobsRestWrapper _adlaJobRestWrapper;
        private AnalyticsAccount account;
        AuthenticatedSession authSession;
        public JobCommands(AnalyticsAccount a, AnalyticsJobsRestWrapper c, AuthenticatedSession authSession)
        {
            this.account = a;
            this._adlaJobRestWrapper = c;
            this.authSession = authSession;
        }

        public ADL.Analytics.Models.JobInformation GetJob(System.Guid jobid)
        {
            var job = this._adlaJobRestWrapper.JobGet(this.account.GetUri(), jobid);
            return job;
        }

        public IEnumerable<ADL.Analytics.Models.JobInformation> GetJobs(GetJobsOptions options)
        {
            var odata_query = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.JobInformation>();

            // if users requests top, set the value appropriately relative to the page size
            if ((options.Top > 0) && (options.Top <= JobCommands.ADLJobPageSize))
            {
                odata_query.Top = options.Top;
            }

            odata_query.OrderBy = options.Sorting.CreateOrderByString();
            odata_query.Filter = options.Filter.ToFilterString(this.authSession);


            var jobs = this._adlaJobRestWrapper.JobList(this.account.GetUri(), odata_query, options.Top);
            foreach (var job in jobs)
            {
                yield return job;
            }
        }

        public ADL.Analytics.Models.JobInformation SubmitJob(SubmitJobOptions options)
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
                options.JobName = "ADL_Demo_Client_Job_" + System.DateTimeOffset.Now.ToString();
            }

            var job_info = this._adlaJobRestWrapper.JobCreate(this.account.GetUri(), options);
            return job_info;
        }

        public ADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            return this._adlaJobRestWrapper.GetStatistics(this.account.GetUri(), jobid);
        }

        public ADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            return this._adlaJobRestWrapper.GetDebugDataPath(this.account.GetUri(), jobid);
        }



    }
}
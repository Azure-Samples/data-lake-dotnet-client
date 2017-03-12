using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsJobClient : AccountClientBase
    {
        // The maximum page size for ADLA list is 300

        public static int ADLJobPageSize = 300;

        private AnalyticsJobsRestClient _adla_job_rest_client;

        AnalyticsAccountUri analyticsuri;

        public AnalyticsJobClient(AnalyticsAccount account, AuthenticatedSession authSession) :
            base(account.Name, authSession)
        {
            this._adla_job_rest_client = new AnalyticsJobsRestClient(this.AuthenticatedSession.Credentials);
            this.analyticsuri = account.GetUri();
        }

        public ADL.Analytics.Models.JobInformation GetJob(System.Guid jobid)
        {
            var job = this._adla_job_rest_client.JobGet(this.analyticsuri, jobid);
            return job;
        }

        public IEnumerable<ADL.Analytics.Models.JobInformation> GetJobs(GetJobsOptions options)
        {
            var odata_query = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.JobInformation>();

            // if users requests top, set the value appropriately relative to the page size
            if ( (options.Top > 0) && (options.Top <= AnalyticsJobClient.ADLJobPageSize))
            {
                odata_query.Top = options.Top;
            }

            odata_query.OrderBy = options.Sorting.CreateOrderByString();
            odata_query.Filter = options.Filter.ToFilterString(this.AuthenticatedSession);

     
            var jobs = this._adla_job_rest_client.JobList(this.analyticsuri, odata_query, options.Top);
            foreach (var job in jobs)
            {
                yield return job;
            }
        }

        public ADL.Analytics.Models.JobInformation  SubmitJob(SubmitJobOptions options)
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

            var job_info = this._adla_job_rest_client.JobCreate(this.analyticsuri, options);
            return job_info;
        }

        public ADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            return this._adla_job_rest_client.GetStatistics(this.analyticsuri, jobid);
        }

        public ADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            return this._adla_job_rest_client.GetDebugDataPath(this.analyticsuri, jobid);
        }
    }
}
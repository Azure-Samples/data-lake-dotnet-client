using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsJobsRestClient
    {
        public ADL.Analytics.DataLakeAnalyticsJobManagementClient _client;
        private Microsoft.Rest.ServiceClientCredentials _creds;

        public AnalyticsJobsRestClient(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._creds = creds;
            this._client = new ADL.Analytics.DataLakeAnalyticsJobManagementClient(this._creds);
        }

        public ADL.Analytics.Models.JobInformation JobGet(string account, System.Guid jobid)
        {
            var job = this._client.Job.Get(account, jobid);
            return job;
        }

        public IEnumerable<ADL.Analytics.Models.JobInformation> JobList(string account,
            Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.JobInformation> odata_query, int top)
        {
            // Other parameters
            string opt_select = null;
            bool? opt_count = null;

            int item_count = 0;
            var page = this._client.Job.List(account, odata_query, opt_select, opt_count);
            foreach (
                var job in
                RESTUtil.EnumItemsInPages<ADL.Analytics.Models.JobInformation>(page,
                    p => this._client.Job.ListNext(p.NextPageLink)))
            {
                yield return job;
                item_count++;

                if ((top > 0) && (item_count >= top))
                {
                    break;
                }
            }

        }

        public ADL.Analytics.Models.JobInformation JobCreate(string account, SubmitJobOptions options)
        {
            var parameters = CreateNewJobProperties(options);
            var job_info = this._client.Job.Create(account, options.JobID, parameters);

            return job_info;

        }

        private static ADL.Analytics.Models.JobInformation CreateNewJobProperties(SubmitJobOptions options)
        {
            var jobprops = new ADL.Analytics.Models.USqlJobProperties();
            jobprops.Script = options.ScriptText;

            var jobType = ADL.Analytics.Models.JobType.USql;
            int priority = 1;
            int dop = 1;

            var parameters = new ADL.Analytics.Models.JobInformation(
                name: options.JobName,
                type: jobType,
                properties: jobprops,
                priority: priority,
                degreeOfParallelism: dop,
                jobId: options.JobID);
            return parameters;
        }

    }

    public class AnalyticsJobClient : AccountClientBase
    {
        // The maximum page size for ADLA list is 300

        public static int ADLJobPageSize = 300;

        private AnalyticsJobsRestClient _adla_job_rest_client;

        public AnalyticsJobClient(string account, AuthenticatedSession authSession) :
            base(account, authSession)
        {
            this._adla_job_rest_client = new AnalyticsJobsRestClient(this.AuthenticatedSession.Credentials);
        }

        public ADL.Analytics.Models.JobInformation GetJob(System.Guid jobid)
        {
            var job = this._adla_job_rest_client.JobGet(this.Account, jobid);
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

     
            var jobs = this._adla_job_rest_client.JobList(this.Account, odata_query, options.Top);
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

            var job_info = this._adla_job_rest_client.JobCreate(this.Account, options);
            return job_info;
        }


        public ADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            var stats = this._adla_job_rest_client._client.Job.GetStatistics(this.Account, jobid);
            return stats;
        }

        public ADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            var jobdatapath = this._adla_job_rest_client._client.Job.GetDebugDataPath(this.Account, jobid);
            return jobdatapath;
        }
    }
}
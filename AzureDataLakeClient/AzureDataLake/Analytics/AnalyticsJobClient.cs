using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsJobClient : AccountClientBase
    {
        // The maximum page size for ADLA list is 300

        public static int ADLJobPageSize = 300;

        private ADL.Analytics.DataLakeAnalyticsJobManagementClient _adla_job_rest_client;

        public AnalyticsJobClient(string account, AuthenticatedSession authSession) :
            base(account, authSession)
        {
            this._adla_job_rest_client = new ADL.Analytics.DataLakeAnalyticsJobManagementClient(this.AuthenticatedSession.Credentials);
        }

        public ADL.Analytics.Models.JobInformation GetJob(System.Guid jobid)
        {
            var job = this._adla_job_rest_client.Job.Get(this.Account, jobid);
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

            // Other parameters
            string opt_select = null;
            bool? opt_count = null;
            string opt_search = null;
            string opt_format = null;

            int item_count = 0;
            var page = this._adla_job_rest_client.Job.List(this.Account, odata_query, opt_select, opt_count, opt_search, opt_format);
            foreach (var job in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.JobInformation>(page, p => this._adla_job_rest_client.Job.ListNext(p.NextPageLink)))
            {
                yield return job;
                item_count++;
                if (item_count >= options.Top)
                {
                    break;
                }
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

            var parameters = CreateNewJobProperties(options);
            var job_info = this._adla_job_rest_client.Job.Create(this.Account, options.JobID, parameters);

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


        public ADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            var stats = this._adla_job_rest_client.Job.GetStatistics(this.Account, jobid);
            return stats;
        }

        public ADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            var jobdatapath = this._adla_job_rest_client.Job.GetDebugDataPath(this.Account, jobid);
            return jobdatapath;
        }
    }
}
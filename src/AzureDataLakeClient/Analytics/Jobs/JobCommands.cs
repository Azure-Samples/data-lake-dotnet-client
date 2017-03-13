using System;
using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics.Jobs
{
    public class JobInfo
    {
        public readonly string Name;
        public readonly string LogFolder;
        public readonly int? AUs;
        public readonly DateTimeOffset? EndTime;
        public readonly IList<JobErrorDetails> ErrorMessage;
        public readonly Guid? Id;
        public readonly IList<string> LogFilePatterns;
        public readonly int? Priority;
        public readonly JobProperties Properties;
        public readonly JobResult? Result;
        public readonly DateTimeOffset? StartTime;
        public readonly JobState? State ;
        public readonly IList<JobStateAuditRecord> StateAuditRecords;
        public readonly DateTimeOffset? SubmitTime;
        public readonly JobType Type;
        public readonly string Submitter;

        public readonly AnalyticsAccount Account;

        public TimeSpan? QueueDuration
        {
            get
            {
                if (this.SubmitTime.HasValue && this.StartTime.HasValue)
                {
                    return this.StartTime.Value- this.SubmitTime.Value;
                }
                return null;
            }
        }

        public TimeSpan? ExecutionDuration
        {
            get
            {
                if (this.StartTime.HasValue && this.EndTime.HasValue)
                {
                    return this.EndTime.Value - this.StartTime.Value;
                }
                return null;
            }
        }

        public double? AUSeconds
        {
            get
            {
                var dur = this.ExecutionDuration;
                if (this.AUs.HasValue && dur.HasValue)
                {
                    return (this.AUs.Value * dur.Value.TotalSeconds);
                }
                return null;
            }
        }

        public JobInfo(JobInformation job, AnalyticsAccount acct)
        {
            this.Account = acct;
            this.Name = job.Name;
            this.LogFolder = job.LogFolder;
            this.LogFolder = job.Submitter;
            this.AUs = job.DegreeOfParallelism;
            this.EndTime = job.EndTime;
            this.ErrorMessage = job.ErrorMessage;
            this.Id = job.JobId;
            this.LogFilePatterns = job.LogFilePatterns;
            this.Priority = job.Priority;
            this.Properties = job.Properties;
            this.Result = job.Result;
            this.StartTime = job.StartTime;
            this.State = job.State;
            this.StateAuditRecords = job.StateAuditRecords;
            this.SubmitTime = job.SubmitTime;
            this.Type = job.Type;
            this.Submitter = job.Submitter;
        }
    }

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

        public IEnumerable<JobInfo> GetJobs(GetJobsOptions options)
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
                var j = new JobInfo(job, this.account);
                yield return j;
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
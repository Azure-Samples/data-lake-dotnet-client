using System;
using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

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
}
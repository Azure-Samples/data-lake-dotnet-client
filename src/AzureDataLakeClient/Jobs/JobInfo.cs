using System;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Jobs
{
    public class JobInfo
    {
        public readonly string Name;
        public readonly string LogFolder;
        public readonly int? AUs;
        public readonly DateTimeOffset? EndTime;
        public readonly Guid? Id;
        public readonly int? Priority;
        public readonly MSADLA.Models.JobResult? Result;
        public readonly DateTimeOffset? StartTime;
        public readonly MSADLA.Models.JobState? State ;
        public readonly DateTimeOffset? SubmitTime;
        public readonly MSADLA.Models.JobType Type;
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


        public JobReference GetJobReference()
        {
            return new JobReference(this.Id.Value, this.Account);
        }       

        internal JobInfo(MSADLA.Models.JobInformation job, AnalyticsAccount acct)
        {
            this.Account = acct;
            this.Name = job.Name;
            this.LogFolder = job.LogFolder;
            this.AUs = job.DegreeOfParallelism;
            this.EndTime = job.EndTime;
            this.Id = job.JobId;
            this.Priority = job.Priority;
            this.Result = job.Result;
            this.StartTime = job.StartTime;
            this.State = job.State;
            this.SubmitTime = job.SubmitTime;
            this.Type = job.Type;
            this.Submitter = job.Submitter;

        }
    }
}
 
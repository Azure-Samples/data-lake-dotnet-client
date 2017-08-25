using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

namespace AdlClient.Models
{
    public class JobDetails : JobInformationBasicEx
    {
        public readonly Microsoft.Azure.Management.DataLake.Analytics.Models.JobProperties Properties;
        public readonly IList<JobErrorDetails> ErrorMessage;
        public readonly IList<JobStateAuditRecord> StateAuditRecords;
        public JobDetailsExtended JobDetailsExtended;

        public JobDetails(Microsoft.Azure.Management.DataLake.Analytics.Models.JobInformation job, AnalyticsAccountRef acct) :
            base(job, acct)
        {
            this.Properties = job.Properties;
            this.ErrorMessage = job.ErrorMessage;
            this.StateAuditRecords = job.StateAuditRecords;
        }
    }
}
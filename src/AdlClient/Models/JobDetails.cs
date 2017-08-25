using System.Collections.Generic;
using MSADLA=Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Models
{
    public class JobDetails : JobInformationBasicEx
    {
        public readonly Microsoft.Azure.Management.DataLake.Analytics.Models.JobProperties Properties;
        public readonly IList<MSADLA.Models.JobErrorDetails> ErrorMessage;
        public readonly IList<MSADLA.Models.JobStateAuditRecord> StateAuditRecords;
        public JobDetailsExtended JobDetailsExtended { get; internal set; }

        public JobDetails(MSADLA.Models.JobInformation job, AnalyticsAccountRef acct) :
            base(job, acct)
        {
            this.Properties = job.Properties;
            this.ErrorMessage = job.ErrorMessage;
            this.StateAuditRecords = job.StateAuditRecords;
        }
    }
}
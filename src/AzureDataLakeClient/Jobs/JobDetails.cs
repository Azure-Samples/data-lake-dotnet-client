using System.Collections.Generic;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Jobs
{
    public class JobDetails
    {
        public JobInfo JobInfo;

        public IList<MSADLA.Models.JobErrorDetails> ErrorMessage;
        public IList<string> LogFilePatterns;
        public IList<Microsoft.Azure.Management.DataLake.Analytics.Models.JobStateAuditRecord> StateAuditRecords;
        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobProperties Properties;

        public JobDetailsExtended JobDetailsExtended;
    }
}
using System.Collections.Generic;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Jobs
{
    public class JobDetails
    {
        public JobInfo JobInfo;

        public IList<MSADLA.Models.JobErrorDetails> ErrorMessage;
        public IList<string> LogFilePatterns;
        public IList<MSADLA.Models.JobStateAuditRecord> StateAuditRecords;
        public MSADLA.Models.JobProperties Properties;

        public JobDetailsExtended JobDetailsExtended;
    }
}
using System.Collections.Generic;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AzureDataLakeClient.Jobs
{
    public class JobExtendedInfo
    {
        public MSADLA.Models.JobProperties Properties;
        public IList<MSADLA.Models.JobErrorDetails> ErrorMessage;
        public IList<string> LogFilePatterns;
        public IList<MSADLA.Models.JobStateAuditRecord> StateAuditRecords;
        public MSADLA.Models.JobStatistics Statistics;
        public MSADLA.Models.JobDataPath DebugDataPath;

        internal JobExtendedInfo(MSADLA.Models.JobInformation job)
        {

            this.Properties = job.Properties;
            this.ErrorMessage = job.ErrorMessage;
            this.LogFilePatterns = job.LogFilePatterns;
            this.StateAuditRecords = job.StateAuditRecords;
        }

    }
}
 
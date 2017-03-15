using System.Collections.Generic;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Jobs
{
    public class ExtendedJobInfo
    {
        public IList<MSADLA.Models.JobErrorDetails> ErrorMessage;
        public IList<string> LogFilePatterns;
        public MSADLA.Models.JobStatistics Statistics;
        public MSADLA.Models.JobDataPath DebugDataPath;
    }
}
 
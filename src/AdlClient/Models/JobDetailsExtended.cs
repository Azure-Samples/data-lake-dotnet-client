using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Models
{
    public class JobDetailsExtended
    {
        public MSADLA.Models.JobStatistics Statistics;
        public MSADLA.Models.JobDataPath DebugDataPath;
    }
}

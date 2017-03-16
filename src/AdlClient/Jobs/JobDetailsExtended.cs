using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Jobs
{
    public class JobDetailsExtended
    {
        public MSADLA.Models.JobStatistics Statistics;
        public MSADLA.Models.JobDataPath DebugDataPath;
    }
}

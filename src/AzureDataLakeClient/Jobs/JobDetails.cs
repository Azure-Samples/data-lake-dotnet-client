using System.Collections.Generic;

namespace AzureDataLakeClient.Jobs
{
    public class JobDetails
    {
        public JobInfo JobInfo;
        public ExtendedJobInfo ExtendedJobInfo;
        public IList<Microsoft.Azure.Management.DataLake.Analytics.Models.JobStateAuditRecord> StateAuditRecords;
        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobProperties Properties;
    }
}
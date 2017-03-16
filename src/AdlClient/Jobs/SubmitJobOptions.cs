using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Jobs
{
    public class SubmitJobOptions
    {
        public System.Guid JobId;
        public string JobName;
        public string ScriptText;
        public int? AUs;
        public int? Priorty;

        public MSADLA.Models.JobInformation ToJobInformationObject()
        {
            var jobprops = new MSADLA.Models.USqlJobProperties();
            jobprops.Script = this.ScriptText;

            var jobType = MSADLA.Models.JobType.USql;
            int dop = 1;

            var job_info = new MSADLA.Models.JobInformation(
                name: this.JobName,
                type: jobType,
                properties: jobprops,
                priority: this.Priorty,
                degreeOfParallelism: this.AUs,
                jobId: this.JobId);
            return job_info;
        }
    }
}
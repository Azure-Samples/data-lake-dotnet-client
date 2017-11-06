using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Models
{
    public class JobCreateParameters
    {
        public System.Guid JobId;
        public string JobName;
        public string ScriptText;
        public int? DegreeOfParallelism;
        public int? Priorty;

        public MSADLA.Models.JobInformation ToJobInformationObject()
        {
            var jobprops = new MSADLA.Models.USqlJobProperties();

            // TODO: Check that script is not null
            jobprops.Script = this.ScriptText;

            // Only U-SQL Jobs supported at the moment
            var jobType = MSADLA.Models.JobType.USql;

            var job_info = new MSADLA.Models.JobInformation(
                name: this.JobName,
                type: jobType,
                properties: jobprops,
                priority: this.Priorty,
                degreeOfParallelism: this.DegreeOfParallelism,
                jobId: this.JobId);
            return job_info;
        }
    }
}
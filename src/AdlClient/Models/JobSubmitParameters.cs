using Microsoft.Azure.Management.DataLake.Analytics.Models;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Models
{
    public class JobSubmitParameters
    {
        public System.Guid JobId;
        public string JobName;
        public int? DegreeOfParallelism;
        public int? Priorty;
        public string ScriptText;
        public JobRelationshipProperties Related;

        public MSADLA.Models.CreateJobParameters ToCreateJobParameters()
        {
            var parameters = new MSADLA.Models.CreateJobParameters();

            // Only U-SQL Jobs supported at the moment
            var jobType = MSADLA.Models.JobType.USql;

            parameters.Name = this.JobName;
            parameters.DegreeOfParallelism = this.DegreeOfParallelism;
            parameters.Priority = this.Priorty;
            parameters.Related = this.Related;
            parameters.Properties = new CreateJobProperties();
            parameters.Properties.Script = this.ScriptText;
            parameters.Type = JobType.USql;

            return parameters;
        }

        public MSADLA.Models.BuildJobParameters ToBuildJobParameters()
        {
            var parameters = new MSADLA.Models.BuildJobParameters();

            // Only U-SQL Jobs supported at the moment
            var jobType = MSADLA.Models.JobType.USql;

            parameters.Name = this.JobName;
            parameters.Properties = new CreateJobProperties();
            parameters.Properties.Script = this.ScriptText;
            parameters.Type = JobType.USql;
            return parameters;
        }

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
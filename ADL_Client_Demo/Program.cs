using System;
using System.Collections.Generic;
using System.Linq;
using AzureDataLakeClient.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

namespace ADL_Client_Demo
{
    class Program
    {
        private static void Main(string[] args)
        {
            var auth_session = new AzureDataLakeClient.Authentication.AuthenticatedSession("ADL_Demo_Client");
            auth_session.Authenticate();

            var client = new AzureDataLakeClient.Analytics.AnalyticsJobClient("datainsightsadhoc", auth_session);

            var opts =new AzureDataLakeClient.Analytics.GetJobsOptions();
            opts.Top = 5;

            var jobfields = new AzureDataLakeClient.Analytics.JobListFields();

            opts.Sorting.Direction = OrderByDirection.Descending;
            opts.Sorting.Field = jobfields.field_submittime;

            opts.Filter.DegreeOfParallelism.OneOf(1,2,10);
            //opts.Filter.SubmitTime.(new System.DateTime(2016, 9, 17));
            //opts.Filter.Priority.Exactly(1);
            //opts.Filter.Result  = new List<JobResult> { JobResult.Cancelled};
            //opts.Filter.State.OneOf(JobState.Ended);
            //opts.Filter.State.Not = true;
            //opts.Filter.SubmitterToCurrentUser = true;
            //opts.Filter.State.OneOf( JobState.Running, JobState.Accepted, JobState.Compiling);
            //opts.Filter.Submitter.OneOf("SAVEENR@microSOFT.COM");
            //opts.Filter.Submitter.IgnoreCase = true;
            //opts.Filter.Submitter.BeginsWith("SaV");
            //opts.Filter.Submitter.IgnoreCase = true;

            var jobs = client.GetJobs(opts);


            foreach (var job in jobs)
            {
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("DoP        = {0}", job.DegreeOfParallelism);
                Console.WriteLine("Result     = {0}", job.Result);
                Console.WriteLine("State      = {0}", job.State);
                Console.WriteLine("SubmitTime = {0} [ Local = {1} ] ", job.SubmitTime.Value, job.SubmitTime.Value.ToLocalTime());
                Console.WriteLine("Priority   = {0}", job.Priority);
                Console.WriteLine("Submitter  = {0}", job.Submitter);
                Console.WriteLine("Name       = {0}", job.Name);                
            }
        }

    }


}

using System.Linq;
using ADLC=AzureDataLakeClient;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests.Analytics
{
    [TestClass]
    public class Analytics_Job_Tests : Base_Tests
    {
        [TestMethod]
        public void Verify_Default_Top()
        {
            this.Initialize();
            var getjobs_options = new ADLC.Analytics.Jobs.GetJobsOptions();

            var jobs = this.adla_account_client.Jobs.GetJobs(getjobs_options).ToList();
            Assert.IsTrue(jobs.Count>2);
        }


        [TestMethod]
        public void Verify_Paging_1()
        {
            this.Initialize();
            var getjobs_options = new ADLC.Analytics.Jobs.GetJobsOptions();
            getjobs_options.Top = 100;

            var jobs = this.adla_account_client.Jobs.GetJobs(getjobs_options).ToList();
            Assert.AreEqual(100,jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_300()
        {
            this.Initialize();
            var getjobs_options = new ADLC.Analytics.Jobs.GetJobsOptions();
            getjobs_options.Top = ADLC.Analytics.Jobs.JobCommands.ADLJobPageSize;

            var jobs = this.adla_account_client.Jobs.GetJobs(getjobs_options).ToList();
            Assert.AreEqual(ADLC.Analytics.Jobs.JobCommands.ADLJobPageSize, jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_400()
        {
            this.Initialize();
            var getjobs_options = new ADLC.Analytics.Jobs.GetJobsOptions();
            var top = ADLC.Analytics.Jobs.JobCommands.ADLJobPageSize + (ADLC.Analytics.Jobs.JobCommands.ADLJobPageSize/2);
            getjobs_options.Top = top;

            var jobs = this.adla_account_client.Jobs.GetJobs(getjobs_options).ToList();
            Assert.AreEqual(top,jobs.Count);
        }


        [TestMethod]
        public void List_Jobs()
        {
            this.Initialize();

            var jobfields = new ADLC.Analytics.Jobs.JobListFields();

            var getjobs_options = new ADLC.Analytics.Jobs.GetJobsOptions();
            getjobs_options.Top = 30;
            getjobs_options.Sorting.Field = jobfields.field_degreeofparallelism;
            getjobs_options.Sorting.Direction = ADLC.Analytics.Jobs.OrderByDirection.Descending;

            foreach (var job in this.adla_account_client.Jobs.GetJobs(getjobs_options))
            {
                System.Console.WriteLine("submitter{0} dop {1}", job.Submitter, job.AUs);
            }
        }

        [TestMethod]
        public void Submit_Job_with_Syntax_Error()
        {
            this.Initialize();
            var sjo = new ADLC.Analytics.Jobs.SubmitJobOptions();
            sjo.ScriptText = "FOOBAR";
            sjo.JobName = "Test Job";
            var ji = this.adla_account_client.Jobs.SubmitJob(sjo);

            System.Console.WriteLine("{0} {1} {2}", ji.Name, ji.JobId, ji.SubmitTime);

            var ji2 = this.adla_account_client.Jobs.GetJob(ji.JobId.Value);

            Assert.AreEqual(ji.Name, ji2.Name);
        }

        [TestMethod]
        public void List_Jobs_Ended()
        {
            this.Initialize();
            var getjobs_options = new ADLC.Analytics.Jobs.GetJobsOptions();
            getjobs_options.Top = 30;
            getjobs_options.Filter.State.OneOf(JobState.Ended);

            var jobs = this.adla_account_client.Jobs.GetJobs(getjobs_options).ToList();
            if (jobs.Count > 0)
            {
                foreach (var job in jobs)
                {
                    Assert.AreEqual(JobState.Ended,job.State);
                }
            }
        }

        [TestMethod]
        public void List_Jobs_Running()
        {
            this.Initialize();
            var getjobs_options = new ADLC.Analytics.Jobs.GetJobsOptions();
            getjobs_options.Top = 30;
            getjobs_options.Filter.State.OneOf(JobState.Running);

            var jobs = this.adla_account_client.Jobs.GetJobs(getjobs_options).ToList();
            if (jobs.Count > 0)
            {
                foreach (var job in jobs)
                {
                    Assert.AreEqual(JobState.Running, job.State);
                }
            }
        }

        [TestMethod]
        public void List_Jobs_Ended_Failed()
        {
            this.Initialize();
            var getjobs_options = new ADLC.Analytics.Jobs.GetJobsOptions();
            getjobs_options.Top = 30;
            getjobs_options.Filter.State.OneOf( JobState.Ended);
            getjobs_options.Filter.Result.OneOf( JobResult.Failed);

            var jobs = this.adla_account_client.Jobs.GetJobs(getjobs_options).ToList();
            if (jobs.Count > 0)
            {
                foreach (var job in jobs)
                {
                    Assert.AreEqual(JobState.Ended, job.State);
                    Assert.AreEqual(JobResult.Failed, job.Result);
                }
            }
        }
    }
}

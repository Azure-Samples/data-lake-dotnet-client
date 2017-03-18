using System.Linq;
using AdlClient.Jobs;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Analytics
{
    [TestClass]
    public class Analytics_Job_Tests : Base_Tests
    {
        [TestMethod]
        public void Verify_Default_Top()
        {
            this.Initialize();
            var getjobs_options = new ListJobOptions();

            var jobs = this.AnalyticsClient.Jobs.ListJobs(getjobs_options).ToList();
            Assert.IsTrue(jobs.Count>2);
        }


        [TestMethod]
        public void Verify_Paging_1()
        {
            this.Initialize();
            var getjobs_options = new ListJobOptions();
            getjobs_options.Top = 100;

            var jobs = this.AnalyticsClient.Jobs.ListJobs(getjobs_options).ToList();
            Assert.AreEqual(100,jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_300()
        {
            this.Initialize();
            var getjobs_options = new ListJobOptions();
            getjobs_options.Top = JobCommands.ADLJobPageSize;

            var jobs = this.AnalyticsClient.Jobs.ListJobs(getjobs_options).ToList();
            Assert.AreEqual(JobCommands.ADLJobPageSize, jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_400()
        {
            this.Initialize();
            var getjobs_options = new ListJobOptions();
            var top = JobCommands.ADLJobPageSize + (JobCommands.ADLJobPageSize/2);
            getjobs_options.Top = top;

            var jobs = this.AnalyticsClient.Jobs.ListJobs(getjobs_options).ToList();
            Assert.AreEqual(top,jobs.Count);
        }


        [TestMethod]
        public void List_Jobs()
        {
            this.Initialize();

            var jobfields = new JobFields();

            var getjobs_options = new ListJobOptions();
            getjobs_options.Top = 30;
            getjobs_options.Sorting.Field = jobfields.DegreeOfParallelism;
            getjobs_options.Sorting.Direction = AdlClient.OData.Enums.OrderByDirection.Descending;

            var jobs = this.AnalyticsClient.Jobs.ListJobs(getjobs_options).ToList();
            foreach (var job in jobs)
            {
                System.Console.WriteLine("submitter{0} dop {1}", job.Submitter, job.DegreeOfParallelism);
            }
        }

        [TestMethod]
        public void Submit_Job_with_Syntax_Error()
        {
            this.Initialize();
            var sjo = new SubmitJobOptions();
            sjo.ScriptText = "FOOBAR";
            sjo.JobName = "Test Job";
            var ji = this.AnalyticsClient.Jobs.SubmitJob(sjo);

            System.Console.WriteLine("{0} {1} {2}", ji.Name, ji.Id, ji.SubmitTime);

            var ji2 = this.AnalyticsClient.Jobs.GetJobDetails(ji.Id.Value,false);

            Assert.AreEqual(ji.Name, ji2.JobInfo.Name);
        }

        [TestMethod]
        public void List_Jobs_Ended()
        {
            this.Initialize();
            var getjobs_options = new ListJobOptions();
            getjobs_options.Top = 30;
            getjobs_options.Filter.State.IsOneOf(JobState.Ended);

            var jobs = this.AnalyticsClient.Jobs.ListJobs(getjobs_options).ToList();
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
            var getjobs_options = new ListJobOptions();
            getjobs_options.Top = 30;
            getjobs_options.Filter.State.IsOneOf(JobState.Running);

            var jobs = this.AnalyticsClient.Jobs.ListJobs(getjobs_options).ToList();
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
            var getjobs_options = new ListJobOptions();
            getjobs_options.Top = 30;
            getjobs_options.Filter.State.IsOneOf( JobState.Ended);
            getjobs_options.Filter.Result.IsOneOf( JobResult.Failed);

            var jobs = this.AnalyticsClient.Jobs.ListJobs(getjobs_options).ToList();
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

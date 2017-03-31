using System;
using System.Linq;
using AdlClient.Commands;
using AdlClient.Models;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Analytics
{
    [TestClass]
    public class Analytics_Profile_Tests : Base_Tests
    {

        class JobVertexProfile
        {
            public System.DateTime? approxEndTime;
            public System.DateTime? versionCreatedTime;
            public System.DateTime? vertexCreateStart;
            public System.DateTime? vertexCreateEnd;
            public System.DateTime? vertexQueueStart;
            public System.DateTime? vertexQueueEnd;
            public System.DateTime? vertexPNQueueStart;
            public System.DateTime? vertexPNQueueEnd;
            public System.DateTime? vertexStartTime;
            public System.DateTime? vertexEndTime;
            public System.DateTime? cleanedUpTime;
            public string vertexGuid;
            public string processId;
            public string vertexId;
            public long bytesRead;
            public long bytesWritten;
            public string vertexResult;
        }

        private static System.DateTime? _getDate(string dateString)
        {
            System.DateTime dt;
            if (System.DateTime.TryParse(dateString, out dt))
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        [TestMethod]
        [DeploymentItem("content\\profiles\\profile_1.txt")]
        public void Parse()
        {
            var filetext = System.IO.File.ReadAllText("profile_1.txt");

            var fileRows = filetext.Split('\n');
            var starttime = System.DateTime.MaxValue;
            int endTime = 0;


            for (var i = 0; i < fileRows.Length; i++)
            {
                var row = fileRows[i];

                var rowColumns = fileRows[i].Split(',');
                int col_count = rowColumns.Length;

                if (rowColumns[0] == "timing")
                {
                    var v = new JobVertexProfile();
                    v.approxEndTime = _getDate(rowColumns[1]);
                    v.versionCreatedTime = _getDate(rowColumns[7]);
                    v.vertexCreateStart = _getDate(rowColumns[8]);
                    v.vertexCreateEnd = _getDate(rowColumns[12]);
                    v.vertexQueueStart = _getDate(rowColumns[19]);
                    v.vertexQueueEnd = _getDate(rowColumns[21]);
                    v.approxEndTime = _getDate(rowColumns[9]);
                    v.vertexPNQueueEnd = _getDate(rowColumns[20]);
                    v.vertexStartTime = _getDate(rowColumns[10]);
                    v.vertexEndTime = _getDate(rowColumns[22]);
                    v.cleanedUpTime = _getDate(rowColumns[11]);
                    v.vertexGuid = rowColumns[3];
                    v.processId = rowColumns[4];
                    v.vertexId = rowColumns[4];
                    v.bytesRead = long.Parse(rowColumns[13]);
                    v.bytesWritten = long.Parse(rowColumns[14]);
                    v.vertexResult = rowColumns[5];

                }

            }

        }
    }

    [TestClass]
    public class Analytics_Job_Tests : Base_Tests
    {

        [TestMethod]
        public void Verify_Paging_1()
        {
            this.Initialize();
            var listing_parameters = new JobListingParameters();
            listing_parameters.Top = 100;

            var jobs = this.AnalyticsClient.Jobs.ListJobs(listing_parameters).ToList();
            Assert.AreEqual(100,jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_300()
        {
            this.Initialize();
            var listing_parameters = new JobListingParameters();
            listing_parameters.Top = 300;

            var jobs = this.AnalyticsClient.Jobs.ListJobs(listing_parameters).ToList();
            Assert.AreEqual(300, jobs.Count);
        }

        [TestMethod]
        public void Verify_Paging_450()
        {
            this.Initialize();
            var listing_parameters = new JobListingParameters();
            listing_parameters.Top = 450;

            var jobs = this.AnalyticsClient.Jobs.ListJobs(listing_parameters).ToList();
            Assert.AreEqual(450, jobs.Count);
        }


        [TestMethod]
        public void List_Jobs()
        {
            this.Initialize();

            var jobfields = new JobFields();

            var listing_parameters = new JobListingParameters();
            listing_parameters.Top = 30;
            listing_parameters.Sorting.Field = jobfields.DegreeOfParallelism;
            listing_parameters.Sorting.Direction = AdlClient.OData.Models.OrderByDirection.Descending;

            var jobs = this.AnalyticsClient.Jobs.ListJobs(listing_parameters).ToList();
            foreach (var job in jobs)
            {
                System.Console.WriteLine("submitter{0} dop {1}", job.Submitter, job.DegreeOfParallelism);
            }
        }

        [TestMethod]
        public void Submit_Job_with_Syntax_Error()
        {
            this.Initialize();
            var submit_parameters = new JobSubmitParameters();
            submit_parameters.ScriptText = "FOOBAR";
            submit_parameters.JobName = "Test Job";
            var ji = this.AnalyticsClient.Jobs.SubmitJob(submit_parameters);

            System.Console.WriteLine("{0} {1} {2}", ji.Name, ji.JobId, ji.SubmitTime);

            var ji2 = this.AnalyticsClient.Jobs.GetJobDetails(ji.JobId,false);

            Assert.AreEqual(ji.Name, ji2.JobInfo.Name);
        }

        [TestMethod]
        public void List_Jobs_Ended()
        {
            this.Initialize();
            var listing_parameters = new JobListingParameters();
            listing_parameters.Top = 30;
            listing_parameters.Filter.State.IsOneOf(JobState.Ended);

            var jobs = this.AnalyticsClient.Jobs.ListJobs(listing_parameters).ToList();
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
            var listing_parameters = new JobListingParameters();
            listing_parameters.Top = 30;
            listing_parameters.Filter.State.IsOneOf(JobState.Running);

            var jobs = this.AnalyticsClient.Jobs.ListJobs(listing_parameters).ToList();
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
            var listing_parameters = new JobListingParameters();
            listing_parameters.Top = 30;
            listing_parameters.Filter.State.IsOneOf( JobState.Ended);
            listing_parameters.Filter.Result.IsOneOf( JobResult.Failed);

            var jobs = this.AnalyticsClient.Jobs.ListJobs(listing_parameters).ToList();
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

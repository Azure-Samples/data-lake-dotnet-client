using System;
using System.Collections.Generic;
using System.Linq;
using ADLC = AzureDataLakeClient;
using MS_ADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace ADL_Client_Demo
{
    class Program
    {
        private static void Main(string[] args)
        {
            var sub = new ADLC.Subscription("045c28ea-c686-462f-9081-33c34e871ba3");
            var rg = new ADLC.ResourceGroup("InsightServices");
            var adla_account = new ADLC.AnalyticsAccount("datainsightsadhoc", sub, rg); // change this to an ADL Analytics account you have access to 
            var adls_account = new ADLC.StoreAccount("datainsightsadhoc", sub, rg); // change this to an ADL Store account you have access to 

            var tenant = new ADLC.Authentication.Tenant("microsoft.onmicrosoft.com"); // change this to YOUR tenant
            var auth_session = new ADLC.Authentication.AuthenticatedSession(tenant);
            auth_session.Authenticate();

            var adla_client = new ADLC.AnalyticsClient(adla_account, auth_session);
            var adls_client = new ADLC.StoreClient(adls_account, auth_session);
            var res_client = new ADLC.ResourceClient(sub, auth_session);

            Demo_Jobs_GetJobUrl(adla_client);

            //Demo_Job_Summaries(adla_client);
            //Demo_Job_Listing(adla_client);
            //Demo_Catalog(adla_client);
            //Demo_Analytics_Account_Management(adla_client);
            //Demo_FileSystem(adls_client);
            //Demo_Resource_Managementr(res_client);
        }

        private static void Demo_Analytics_Account_Management(ADLC.AnalyticsClient adla_client)
        {
            Demo_AnalyticsAccount_List_LinkedStoreAccounts(adla_client);
        }

        private static void Demo_FileSystem(ADLC.StoreClient adls_client)
        {
            Demo_FileSystem_ListFilesAtRoot(adls_client);
        }

        private static void Demo_Resource_Managementr(ADLC.ResourceClient res_client)
        {
            Demo_Resource_List_AnalyticsAccounts(res_client);
            Demo_Resource_ListDataLakeStoreAccountsInSubscription(res_client);
            Demo_Resource_ListResourceGroups(res_client);
        }

        private static void Demo_Catalog(ADLC.AnalyticsClient adla_client)
        {
            Demo_Catalog_ListDatabases(adla_client);
        }

        private static void Demo_Job_Listing(ADLC.AnalyticsClient adla_client)
        {
            Demo_Jobs_List_NeverStarted(adla_client);
            Demo_Jobs_List_Recent(adla_client);
            Demo_Jobs_List_SingleMostRecent(adla_client);
            Demo_Jobs_List_Oldest(adla_client);
            Demo_Jobs_List_Failed(adla_client);
            Demo_Jobs_List_SubmittedBy_AuthenticatedUser(adla_client);
            Demo_Jobs_List_SubmittedBy_Users(adla_client);
            Demo_Jobs_List_SubmittedBetween_MidnightAndNow(adla_client);
            Demo_Jobs_List_SubmittedBy_UserBeginsWith(adla_client);
            Demo_Jobs_List_SubmittedBy_UserContains(adla_client);
            Demo_Jobs_List_MostExpensive_In_Last24hours(adla_client);
        }

        private static void Demo_Job_Summaries(ADLC.AnalyticsClient adla_client)
        {
            Demo_Jobs_Summarize_FailedAUHours_By_Submitter(adla_client);
            Demo_Jobs_Summarize_AUHours_By_JobResult_nad_Submitter(adla_client);
        }

        private static void Demo_FileSystem_ListFilesAtRoot(ADLC.StoreClient adls_client)
        {
            //var root = ADLC.Store.FsPath.Root; // same as "/"
            var root = new ADLC.FileSystem.FsPath("/Samples");
            var lfo = new ADLC.FileSystem.ListFilesOptions();
            foreach (var page in adls_client.FileSystem.ListFilesPaged(root,lfo))
            {
                foreach (var fileitemn in page.FileItems)
                {
                    Console.WriteLine("path={0} filename={1}",page.Path,fileitemn.PathSuffix);                    
                }
            }
        }

        private static void Demo_Jobs_GetJobUrl(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 1;
            var job = adla_client.Jobs.GetJobs(opts).First();


            string joburi_string = job.GetUri();
            string job_portal_link_string = job.GetAzurePortalLink();
                
            Console.WriteLine(joburi_string);
            Console.WriteLine();
            Console.WriteLine(job_portal_link_string);
        }


        private static void Demo_Jobs_List_SingleMostRecent(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 1;
            var jobs = adla_client.Jobs.GetJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_SubmittedBy_AuthenticatedUser(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 10;
            opts.Filter.Submitter.IsOneOf(adla_client.AuthenticatedSession.Token.DisplayableId);

            var jobs = adla_client.Jobs.GetJobs(opts).ToList();

            PrintJobs(jobs);
        }


        private static void Demo_Jobs_List_SubmittedBy_Users(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 10;
            opts.Filter.Submitter.IsOneOf("mrys@microsoft.com", "saveenr@microsoft.com");

            var jobs = adla_client.Jobs.GetJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_SubmittedBy_UserBeginsWith(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 10;
            opts.Filter.Submitter.BeginsWith("saa");

            var jobs = adla_client.Jobs.GetJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_SubmittedBy_UserContains(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 10;
            opts.Filter.Submitter.Contains("eenr");

            var jobs = adla_client.Jobs.GetJobs(opts).ToList();

            PrintJobs(jobs);
        }


        private static void Demo_Jobs_List_Recent(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 10;

            var jobfields = new ADLC.Jobs.JobListFields();
            opts.Sorting.Direction = ADLC.Jobs.OrderByDirection.Descending;
            opts.Sorting.Field = jobfields.field_submittime;

            var jobs = adla_client.Jobs.GetJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_Oldest(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 10;

            var jobfields = new ADLC.Jobs.JobListFields();
            opts.Sorting.Direction = ADLC.Jobs.OrderByDirection.Ascending;
            opts.Sorting.Field = jobfields.field_submittime;

            var jobs = adla_client.Jobs.GetJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_Failed(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 5;

            opts.Filter.Result.IsOneOf(MS_ADLA.Models.JobResult.Failed);

            var jobs = adla_client.Jobs.GetJobs(opts).ToList();

            PrintJobs(jobs);
        }


        private static void Demo_GetJobsSubmitedInLast2hours(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Filter.SubmitTime.InRange(ADLC.OData.Utils.RangeDateTime.InTheLastNHours(2));
            var jobs = adla_client.Jobs.GetJobs(opts).ToList();
            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_SubmittedBetween_MidnightAndNow(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Filter.SubmitTime.InRange(ADLC.OData.Utils.RangeDateTime.SinceLocalMidnight());
            var jobs = adla_client.Jobs.GetJobs(opts).ToList();
            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_NeverStarted(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 2;
            opts.Filter.StartTime.IsNull();
            var jobs = adla_client.Jobs.GetJobs(opts).ToList().ToList();
            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_MostExpensive_In_Last24hours(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Filter.SubmitTime.InRange(AzureDataLakeClient.OData.Utils.RangeDateTime.InTheLastNHours(24));
            var jobs = adla_client.Jobs.GetJobs(opts).OrderByDescending(j=>j.AUSeconds).Take(10).ToList();

            PrintJobs(jobs);
        }

        private static void PrintJobs(IEnumerable<ADLC.Jobs.JobInfo> jobs)
        {
            foreach (var job in jobs)
            {
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("Name = {0}", job.Name);
                Console.WriteLine("AUs = {0}", job.AUs);
                Console.WriteLine("Priority = {0}", job.Priority);
                Console.WriteLine("Result = {0}; State = {1}", job.Result, job.State);
                Console.WriteLine("StartTime = {0} ", job.SubmitTime);
                Console.WriteLine("SubmitTime = {0} [ Local = {1} ] ", job.SubmitTime.Value, job.SubmitTime.Value.ToLocalTime());
                Console.WriteLine("Submitter = {0}", job.Submitter);
                Console.WriteLine("AUHours = {0}", job.AUSeconds / (60.0 * 60.0));
            }
        }

        private static void Demo_AnalyticsAccount_List_LinkedStoreAccounts(ADLC.AnalyticsClient adla_client)
        {
            var storage_accounts = adla_client.Management.ListLinkedDataLakeStoreAccounts().ToList();
            foreach (var i in storage_accounts)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Type = {0}", i.Type);
            }
        }

        private static void Demo_Resource_List_AnalyticsAccounts(ADLC.ResourceClient res_client)
        {
            var storage_accounts = res_client.Analytics.ListAccounts().ToList();
            foreach (var i in storage_accounts)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Location = {0}", i.Location);
                Console.WriteLine("Type = {0}", i.Type);
            }
        }

        private static void Demo_Catalog_ListDatabases(ADLC.AnalyticsClient adla_client)
        {
            var databases = adla_client.Catalog.ListDatabases().ToList();
            foreach (var i in databases)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Version = {0}", i.Version);
                Console.WriteLine("ComputeAccountName = {0}", i.ComputeAccountName);
            }
        }

        private static void Demo_Resource_ListDataLakeStoreAccountsInSubscription(ADLC.ResourceClient res_client)
        {
            var storage_accounts = res_client.Store.ListAccounts().ToList();
            foreach (var i in storage_accounts)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Location = {0}", i.Location);
                Console.WriteLine("Type = {0}", i.Type);
            }
        }

        private static void Demo_Resource_ListResourceGroups(ADLC.ResourceClient res_client)
        {
            var rgs = res_client.ListResourceGroups().ToList();
            foreach (var i in rgs)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Location = {0}", i.Location);
            }
        }


        private static void Demo_Jobs_Summarize_FailedAUHours_By_Submitter(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 300;

            opts.Filter.Result.IsOneOf(MS_ADLA.Models.JobResult.Failed);
            opts.Filter.StartTime.IsNotNull();

            var failed_jobs = adla_client.Jobs.GetJobs(opts).ToList();

            var results = from job in failed_jobs
                          group job by job.Submitter into job_group
                          select new 
                          {
                              Submitter = job_group.Key,
                              Count = job_group.Count(),
                              AUHours = job_group.Sum( j=> j.AUSeconds)/(60.0*60.0),
                          };

            foreach (var i in results)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Submitter = {0}", i.Submitter);
                Console.WriteLine("NumJobs = {0}", i.Count);
                Console.WriteLine("AU Hours = {0}", i.AUHours);
            }

        }

        private static void Demo_Jobs_Summarize_AUHours_By_JobResult_nad_Submitter(ADLC.AnalyticsClient adla_client)
        {
            var opts = new ADLC.Jobs.GetJobsOptions();
            opts.Top = 300;

            //opts.Filter.Result.OneOf(MS_ADLA.Models.JobResult.Failed);

            var jobs = adla_client.Jobs.GetJobs(opts).Where(j => j.StartTime != null).ToList();

            var results = from job in jobs
                          group job by 
                            new {   job.Result,
                                    job.Submitter }  
                            into job_group
                          select new
                          {
                              Submitter = job_group.Key.Submitter,
                              Result = job_group.Key.Result,
                              Count = job_group.Count(),
                              AUHours = job_group.Sum(j => j.AUSeconds) / (60.0 * 60.0),
                          };

            foreach (var i in results)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Submitter = {0}", i.Submitter);
                Console.WriteLine("Result = {0}", i.Result);
                Console.WriteLine("NumJobs = {0}", i.Count);
                Console.WriteLine("AU Hours = {0}", i.AUHours);
            }

        }

    }
}

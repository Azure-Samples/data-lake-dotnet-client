using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace DemoAdlClient
{
    class Program
    {
        private static void Main(string[] args)
        {
            // Collect info about the Azure resources needed for this demo
            var sub = new AdlClient.Subscription("045c28ea-c686-462f-9081-33c34e871ba3");
            var rg = new AdlClient.ResourceGroup("InsightServices");
            var adla_account = new AdlClient.AnalyticsAccount(sub, rg, "datainsightsadhoc"); // change this to an ADL Analytics account you have access to 
            var adls_account = new AdlClient.StoreAccount(sub, rg, "datainsightsadhoc"); // change this to an ADL Store account you have access to 

            // Setup authentication for this demo
            var tenant = new AdlClient.Authentication.Tenant("microsoft.onmicrosoft.com"); // change this to YOUR tenant
            var auth = new AdlClient.Authentication.AuthenticatedSession(tenant);
            auth.Authenticate();

            // Create the clients
            var adla_client = new AdlClient.AnalyticsClient(adla_account, auth);
            var adls_client = new AdlClient.StoreClient(adls_account, auth);
            var az_client = new AdlClient.AzureClient(sub, auth);

            // ------------------------------
            // Run the Demo
            // ------------------------------

            Demo_JobsDetails(adla_client);
            Demo_Jobs_GetJobUrl(adla_client);
            Demo_Job_Summaries(adla_client);
            Demo_Job_Listing(adla_client);
            Demo_Catalog(adla_client);
            Demo_Analytics_Account_Management(adla_client);
            Demo_FileSystem(adls_client);
            Demo_Resource_Managementr(az_client);
        }

        private static void Demo_JobsDetails(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 5;
            opts.Filter.State.IsOneOf( JobState.Ended );
            opts.Filter.Result.IsOneOf( JobResult.Succeeded );
            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            var first_job = jobs[0];
            var jobdetails = adla_client.Jobs.GetJobDetails(first_job.Id.Value, true);
        }

        private static void Demo_Analytics_Account_Management(AdlClient.AnalyticsClient adla_client)
        {
            Demo_AnalyticsAccount_List_LinkedStoreAccounts(adla_client);
        }

        private static void Demo_FileSystem(AdlClient.StoreClient adls_client)
        {
            Demo_FileSystem_ListFilesInFolder(adls_client);
        }

        private static void Demo_Resource_Managementr(AdlClient.AzureClient res_client)
        {
            Demo_Resource_List_AnalyticsAccounts(res_client);
            Demo_Resource_ListDataLakeStoreAccountsInSubscription(res_client);
            Demo_Resource_ListResourceGroups(res_client);
        }

        private static void Demo_Catalog(AdlClient.AnalyticsClient adla_client)
        {
            Demo_Catalog_ListDatabases(adla_client);
        }

        private static void Demo_Job_Listing(AdlClient.AnalyticsClient adla_client)
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

        private static void Demo_Job_Summaries(AdlClient.AnalyticsClient adla_client)
        {
            Demo_Jobs_Summarize_FailedAUHours_By_Submitter(adla_client);
            Demo_Jobs_Summarize_AUHours_By_JobResult_nad_Submitter(adla_client);
        }

        private static void Demo_FileSystem_ListFilesInFolder(AdlClient.StoreClient adls_client)
        {
            //var folder = ADLC.Store.FsPath.Root; // same as "/"
            var folder = new AdlClient.FileSystem.FsPath("/Samples");
            var lfo = new AdlClient.FileSystem.ListFilesOptions();
            foreach (var page in adls_client.FileSystem.ListFilesPaged(folder,lfo))
            {
                foreach (var fileitemn in page.FileItems)
                {
                    Console.WriteLine("path={0} filename={1}",page.Path,fileitemn.PathSuffix);                    
                }
            }
        }

        private static void Demo_Jobs_GetJobUrl(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 3;
            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            foreach (var job in jobs)
            {
                var joblink = job.GetJobReference();

                string joburi_string = joblink.GetUri();
                string job_portal_link_string = joblink.GetAzurePortalLink();

                Console.WriteLine(joburi_string);
                Console.WriteLine(job_portal_link_string);
            }
        }


        private static void Demo_Jobs_List_SingleMostRecent(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 1;
            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_SubmittedBy_AuthenticatedUser(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 10;
            opts.Filter.Submitter.IsOneOf(adla_client.AuthenticatedSession.Token.DisplayableId);

            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            PrintJobs(jobs);
        }


        private static void Demo_Jobs_List_SubmittedBy_Users(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 10;
            opts.Filter.Submitter.IsOneOf("mrys@microsoft.com", "saveenr@microsoft.com");

            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_SubmittedBy_UserBeginsWith(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 10;
            opts.Filter.Submitter.BeginsWith("saa");

            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_SubmittedBy_UserContains(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 10;
            opts.Filter.Submitter.Contains("eenr");

            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            PrintJobs(jobs);
        }


        private static void Demo_Jobs_List_Recent(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 10;

            var jobfields = new AdlClient.Jobs.ListJobFields();
            opts.Sorting.Direction = AdlClient.OData.Enums.OrderByDirection.Descending;
            opts.Sorting.Field = jobfields.field_submittime;

            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_Oldest(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 10;

            var jobfields = new AdlClient.Jobs.ListJobFields();
            opts.Sorting.Direction = AdlClient.OData.Enums.OrderByDirection.Ascending;
            opts.Sorting.Field = jobfields.field_submittime;

            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_Failed(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 5;

            opts.Filter.Result.IsOneOf(MSADLA.Models.JobResult.Failed);

            var jobs = adla_client.Jobs.ListJobs(opts).ToList();

            PrintJobs(jobs);
        }


        private static void Demo_GetJobsSubmitedInLast2hours(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Filter.SubmitTime.InRange(AdlClient.OData.Utils.RangeDateTime.InTheLastNHours(2));
            var jobs = adla_client.Jobs.ListJobs(opts).ToList();
            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_SubmittedBetween_MidnightAndNow(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Filter.SubmitTime.InRange(AdlClient.OData.Utils.RangeDateTime.SinceLocalMidnight());
            var jobs = adla_client.Jobs.ListJobs(opts).ToList();
            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_NeverStarted(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 2;
            opts.Filter.StartTime.IsNull();
            var jobs = adla_client.Jobs.ListJobs(opts).ToList().ToList();
            PrintJobs(jobs);
        }

        private static void Demo_Jobs_List_MostExpensive_In_Last24hours(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Filter.SubmitTime.InRange(AdlClient.OData.Utils.RangeDateTime.InTheLastNHours(24));
            var jobs = adla_client.Jobs.ListJobs(opts).OrderByDescending(j=>j.AUSeconds).Take(10).ToList();

            PrintJobs(jobs);
        }

        private static void PrintJobs(IEnumerable<AdlClient.Jobs.JobInfo> jobs)
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

        private static void Demo_AnalyticsAccount_List_LinkedStoreAccounts(AdlClient.AnalyticsClient adla_client)
        {
            var storage_accounts = adla_client.Management.ListLinkedDataLakeStoreAccounts().ToList();
            foreach (var i in storage_accounts)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Type = {0}", i.Type);
            }
        }

        private static void Demo_Resource_List_AnalyticsAccounts(AdlClient.AzureClient res_client)
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

        private static void Demo_Catalog_ListDatabases(AdlClient.AnalyticsClient adla_client)
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

        private static void Demo_Resource_ListDataLakeStoreAccountsInSubscription(AdlClient.AzureClient res_client)
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

        private static void Demo_Resource_ListResourceGroups(AdlClient.AzureClient res_client)
        {
            var rgs = res_client.ListResourceGroups().ToList();
            foreach (var i in rgs)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Location = {0}", i.Location);
            }
        }


        private static void Demo_Jobs_Summarize_FailedAUHours_By_Submitter(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 300;

            opts.Filter.Result.IsOneOf(MSADLA.Models.JobResult.Failed);
            opts.Filter.StartTime.IsNotNull();

            var failed_jobs = adla_client.Jobs.ListJobs(opts).ToList();

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

        private static void Demo_Jobs_Summarize_AUHours_By_JobResult_nad_Submitter(AdlClient.AnalyticsClient adla_client)
        {
            var opts = new AdlClient.Jobs.ListJobOptions();
            opts.Top = 300;

            //opts.Filter.Result.OneOf(MS_ADLA.Models.JobResult.Failed);

            var jobs = adla_client.Jobs.ListJobs(opts).Where(j => j.StartTime != null).ToList();

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

            foreach (var row in results)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Submitter = {0}", row.Submitter);
                Console.WriteLine("Result = {0}", row.Result);
                Console.WriteLine("NumJobs = {0}", row.Count);
                Console.WriteLine("AU Hours = {0}", row.AUHours);
            }

        }

    }
}

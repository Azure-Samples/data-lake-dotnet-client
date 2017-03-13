using System;
using System.Collections.Generic;
using System.Linq;
using AzureDataLakeClient.FileSystem;
using AzureDataLakeClient.Jobs;
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

            var adla_client = new ADLC.AnalyticsAccountClient(adla_account, auth_session);
            var adls_client = new ADLC.StoreAccountClient(adls_account, auth_session);
            var sub_client = new ADLC.SubscriptionClient(sub, auth_session);

            //Demo_GetExactlyOneJob(adla_client);
            //Demo_Get10OldestJobs(adla_client);
            //Demo_Get10MostRecentJobs(adla_client);
            //Demo_Get5FailedJobs(adla_client);
            //Demo_GetJobsSubmittedByMe(adla_client);
            //Demo_GetJobsSubmittedByUsers(adla_client);
            //Demo_GetJobsSubmitedSinceMidnight(adla_client);
            //Demo_GetJobs_Submitter_Begins_With(adla_client);
            //Demo_GetJobs_Submitter_Contains(adla_client);
            Demo_GetTop10MostExpensiveSubmitedInLast24hours(adla_client);
            //
            //Demo_ListFilesAtRoot(adls_client);
            //Demo_ListLinkedDataLakeStoreAccounts(adla_client);
            //
            //Demo_ListDataLakeAnalyticsAccountsInSubscription(sub_client);
            //Demo_ListDatabases(adla_client);
            //Demo_ListDataLakeStoreAccountsInSubscription(sub_client);
        }

        private static void Demo_ListFilesAtRoot(ADLC.StoreAccountClient adls_client)
        {
            //var root = ADLC.Store.FsPath.Root; // same as "/"
            var root = new FsPath("/Samples");
            var lfo = new ListFilesOptions();
            foreach (var page in adls_client.FileSystem.ListFilesPaged(root,lfo))
            {
                foreach (var fileitemn in page.FileItems)
                {
                    Console.WriteLine("path={0} filename={1}",page.Path,fileitemn.PathSuffix);                    
                }
            }

        }

        private static void Demo_GetExactlyOneJob(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Top = 1;
            var jobs = adla_client.Jobs.GetJobs(opts);

            PrintJobs(jobs);
        }

        private static void Demo_GetJobsSubmittedByAuthenticatedUser(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Top = 10;
            opts.Filter.Submitter.OneOf(adla_client.AuthenticatedSession.Token.DisplayableId);

            var jobs = adla_client.Jobs.GetJobs(opts);

            PrintJobs(jobs);
        }


        private static void Demo_GetJobsSubmittedByUsers(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Top = 10;
            opts.Filter.Submitter.OneOf("mrys@microsoft.com", "saveenr@microsoft.com");

            var jobs = adla_client.Jobs.GetJobs(opts);

            PrintJobs(jobs);
        }

        private static void Demo_GetJobs_Submitter_Begins_With(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Top = 10;
            opts.Filter.Submitter.BeginsWith("saa");

            var jobs = adla_client.Jobs.GetJobs(opts);

            PrintJobs(jobs);
        }

        private static void Demo_GetJobs_Submitter_Contains(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Top = 10;
            opts.Filter.Submitter.Contains("eenr");

            var jobs = adla_client.Jobs.GetJobs(opts);

            PrintJobs(jobs);
        }


        private static void Demo_Get10MostRecentJobs(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Top = 10;

            var jobfields = new JobListFields();
            opts.Sorting.Direction = OrderByDirection.Descending;
            opts.Sorting.Field = jobfields.field_submittime;

            var jobs = adla_client.Jobs.GetJobs(opts);

            PrintJobs(jobs);
        }

        private static void Demo_Get10OldestJobs(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Top = 10;

            var jobfields = new JobListFields();
            opts.Sorting.Direction = OrderByDirection.Ascending;
            opts.Sorting.Field = jobfields.field_submittime;

            var jobs = adla_client.Jobs.GetJobs(opts);

            PrintJobs(jobs);
        }

        private static void Demo_Get5FailedJobs(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Top = 5;

            opts.Filter.Result.OneOf(MS_ADLA.Models.JobResult.Failed);

            var jobs = adla_client.Jobs.GetJobs(opts);

            PrintJobs(jobs);
        }

        private static void Demo_GetJobsSubmitedInLast2hours(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Filter.SubmitTime.InRange(AzureDataLakeClient.OData.Utils.RangeDateTime.InTheLastNHours(2));
            var jobs = adla_client.Jobs.GetJobs(opts);
            PrintJobs(jobs);
        }

        private static void Demo_GetJobsSubmitedSinceMidnight(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Filter.SubmitTime.InRange(AzureDataLakeClient.OData.Utils.RangeDateTime.SinceLocalMidnight());
            var jobs = adla_client.Jobs.GetJobs(opts);
            PrintJobs(jobs);
        }

        private static void Demo_GetTop10MostExpensiveSubmitedInLast24hours(ADLC.AnalyticsAccountClient adla_client)
        {
            var opts = new GetJobsOptions();
            opts.Filter.SubmitTime.InRange(AzureDataLakeClient.OData.Utils.RangeDateTime.InTheLastNHours(24));
            var jobs = adla_client.Jobs.GetJobs(opts).OrderByDescending(j=>j.AUSeconds).Take(10).ToList();

            PrintJobs(jobs);
        }

        private static void PrintJobs(IEnumerable<JobInfo> jobs)
        {
            foreach (var job in jobs)
            {
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("Name = {0}", job.Name);
                Console.WriteLine("AUs = {0}", job.AUs);
                Console.WriteLine("Priority = {0}", job.Priority);
                Console.WriteLine("Result = {0}; State = {1}", job.Result, job.State);
                Console.WriteLine("SubmitTime = {0} [ Local = {1} ] ", job.SubmitTime.Value, job.SubmitTime.Value.ToLocalTime());
                Console.WriteLine("Submitter = {0}", job.Submitter);
                Console.WriteLine("AUHours = {0}", job.AUSeconds / (60.0 * 60.0));
            }
        }

        private static void Demo_ListLinkedDataLakeStoreAccounts(ADLC.AnalyticsAccountClient adla_client)
        {
            var storage_accounts = adla_client.Management.ListLinkedDataLakeStoreAccounts().ToList();
            foreach (var i in storage_accounts)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Type = {0}", i.Type);
            }
        }

        private static void Demo_ListDataLakeAnalyticsAccountsInSubscription(ADLC.SubscriptionClient sub_client)
        {
            var storage_accounts = sub_client.ListAnalyticsAccounts().ToList();
            foreach (var i in storage_accounts)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Location = {0}", i.Location);
                Console.WriteLine("Type = {0}", i.Type);
            }
        }

        private static void Demo_ListDatabases(ADLC.AnalyticsAccountClient adla_client)
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

        private static void Demo_ListDataLakeStoreAccountsInSubscription(ADLC.SubscriptionClient sub_client)
        {
            var storage_accounts = sub_client.ListStoreAccounts().ToList();
            foreach (var i in storage_accounts)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Name = {0}", i.Name);
                Console.WriteLine("Location = {0}", i.Location);
                Console.WriteLine("Type = {0}", i.Type);
            }
        }
    }
}

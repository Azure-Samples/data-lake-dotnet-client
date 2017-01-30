namespace AzureDataLakeClient.Analytics
{
    public class GetJobsOptions
    {
        public int Top=100; // 300 is the ADLA limit
        public JobListFilter Filter;
        public JobListSorting Sorting;

        public GetJobsOptions()
        {
            this.Filter = new JobListFilter();
            this.Sorting = new JobListSorting();
        }
    }
}
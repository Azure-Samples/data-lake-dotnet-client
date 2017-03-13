namespace AzureDataLakeClient.Jobs
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

            var jobfields = new JobListFields();
            this.Sorting.Direction = OrderByDirection.Descending;
            this.Sorting.Field = jobfields.field_submittime;
        }
    }
}
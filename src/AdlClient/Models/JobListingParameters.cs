namespace AdlClient.Models
{
    public class JobListingParameters
    {
        public int Top=100; // 300 is the ADLA limit
        public JobListingFilter Filter;
        public JobListingSorting Sorting;

        public JobListingParameters()
        {
            this.Filter = new JobListingFilter();
            this.Sorting = new JobListingSorting();

            var jobfields = new JobExprFields();
            this.Sorting.Direction = AdlClient.OData.Models.OrderByDirection.Descending;
            this.Sorting.Field = jobfields.SubmitTime;
        }
    }
}
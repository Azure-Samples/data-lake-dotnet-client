namespace AdlClient.Models
{
    public class JobListingParameters
    {
        // 100 is picked as the default number of records to 
        // retrieve so that vallers are overwhelmed with data on first use

        public int Top = 100; 
        public JobListingFilter Filter;
        public JobListingSorting Sorting;

        public JobListingParameters()
        {
            this.Filter = new JobListingFilter();
            this.Sorting = new JobListingSorting();

            this.Sorting.BySubmitTime(AdlClient.OData.Models.OrderByDirection.Descending);
        }
    }
}
namespace AdlClient.Models
{
    public class JobListParameters
    {
        // 100 is picked as the default number of records to 
        // retrieve so that vallers are overwhelmed with data on first use

        public int Top = 100; 
        public JobListFilter Filter;
        public JobListSorting Sorting;

        public JobListParameters()
        {
            this.Filter = new JobListFilter();
            this.Sorting = new JobListSorting();

            this.Sorting.BySubmitTime(AdlClient.OData.Models.OrderByDirection.Descending);
        }
    }
}
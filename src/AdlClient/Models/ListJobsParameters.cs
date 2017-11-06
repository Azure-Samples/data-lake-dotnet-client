namespace AdlClient.Models
{
    public class ListJobsParameters
    {
        // 100 is picked as the default number of records to 
        // retrieve so that vallers are overwhelmed with data on first use

        public int Top = 100; 
        public ListJobsFilter Filter;
        public ListJobsSorting Sorting;

        public ListJobsParameters()
        {
            this.Filter = new ListJobsFilter();
            this.Sorting = new ListJobsSorting();

            this.Sorting.BySubmitTime(AdlClient.OData.Models.OrderByDirection.Descending);
        }
    }
}
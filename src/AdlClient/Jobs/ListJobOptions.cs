namespace AdlClient.Jobs
{
    public class ListJobOptions
    {
        public int Top=100; // 300 is the ADLA limit
        public ListJobFilter Filter;
        public ListJobSorting Sorting;

        public ListJobOptions()
        {
            this.Filter = new ListJobFilter();
            this.Sorting = new ListJobSorting();

            var jobfields = new ListJobFields();
            this.Sorting.Direction = AdlClient.OData.Enums.OrderByDirection.Descending;
            this.Sorting.Field = jobfields.field_submittime;
        }
    }
}
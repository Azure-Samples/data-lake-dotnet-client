namespace AzureDataLakeClient.Analytics
{
    public class JobListSorting
    {
        public AzureDataLakeClient.OData.ExprField Field;
        public OrderByDirection Direction;
        
        public string CreateOrderByString()
        {
            if (this.Field != null)
            {
                var dir = DirectionToString(this.Direction);
                string orderBy = string.Format("{0} {1}", this.Field.Name, dir);
                return orderBy;
            }

            return null;
        }

        private static string DirectionToString(OrderByDirection direction)
        {
            var dir = (direction == OrderByDirection.Ascending) ? "asc" : "desc";
            return dir;
        }
    }

    public class GetJobsOptions
    {
        public int Top=0; // 300 is the ADLA limit
        public JobListFilter Filter;
        public JobListSorting Sorting;

        public GetJobsOptions()
        {
            this.Filter = new JobListFilter();
            this.Sorting = new JobListSorting();
        }
    }
}
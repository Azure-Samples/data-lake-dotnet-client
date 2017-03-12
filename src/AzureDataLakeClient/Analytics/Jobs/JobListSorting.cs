namespace AzureDataLakeClient.Analytics.Jobs
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
}
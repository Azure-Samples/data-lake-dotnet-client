using AdlClient.OData;

namespace AdlClient.Jobs
{
    public class ListJobSorting
    {
        public ExprField Field;
        public AdlClient.OData.Enums.OrderByDirection Direction;
        
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

        private static string DirectionToString(AdlClient.OData.Enums.OrderByDirection direction)
        {
            var dir = (direction == AdlClient.OData.Enums.OrderByDirection.Ascending) ? "asc" : "desc";
            return dir;
        }
    }
}
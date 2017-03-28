using AdlClient.OData;

namespace AdlClient.Models
{
    public class JobListingSorting
    {
        public ExprField Field;
        public AdlClient.OData.Models.OrderByDirection Direction;
        
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

        private static string DirectionToString(AdlClient.OData.Models.OrderByDirection direction)
        {
            var dir = (direction == AdlClient.OData.Models.OrderByDirection.Ascending) ? "asc" : "desc";
            return dir;
        }
    }
}
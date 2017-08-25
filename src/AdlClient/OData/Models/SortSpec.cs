using AdlClient.OData;

namespace AdlClient.OData.Models
{
    public class SortSpec
    {
        public ExprField Field { get; private set; }
        public OrderByDirection Direction { get; private set; }

        public SortSpec(ExprField field, AdlClient.OData.Models.OrderByDirection direction)
        {
            this.Field = field;
            this.Direction = direction;
        }

        public string CreateOrderByString()
        {
            var dir = DirectionToString(this.Direction);
            string orderBy = string.Format("{0} {1}", this.Field.Name, dir);
            return orderBy;
        }

        private static string DirectionToString(OrderByDirection direction)
        {
            var dir = (direction == OrderByDirection.Ascending) ? "asc" : "desc";
            return dir;
        }

    }
}
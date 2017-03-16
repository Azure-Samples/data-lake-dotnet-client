using AdlClient.OData;

namespace AdlClient.Jobs
{
    public class ListJobSorting
    {
        public ExprField Field;
        public ListJobOrderyByDirection Direction;
        
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

        private static string DirectionToString(ListJobOrderyByDirection direction)
        {
            var dir = (direction == ListJobOrderyByDirection.Ascending) ? "asc" : "desc";
            return dir;
        }
    }
}
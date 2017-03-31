using AdlClient.OData;

namespace AdlClient.Models
{
    public class JobListingSorting
    {
        public ExprField Field { get; private set; }
        public AdlClient.OData.Models.OrderByDirection Direction { get; private set; }

        static JobExprFields jobfields = new JobExprFields();

        internal string CreateOrderByString()
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

        public void BySubmitTime(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Field = jobfields.SubmitTime;
            this.Direction = dir;
        }


        public void ByEndTime(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Field = jobfields.EndTime;
            this.Direction = dir;
        }

        public void ByName(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Field = jobfields.Name;
            this.Direction = dir;
        }

        public void ByPriority(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Field = jobfields.Priority;
            this.Direction = dir;
        }

        public void ByResult(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Field = jobfields.Result;
            this.Direction = dir;
        }

        public void ByStartTime(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Field = jobfields.StartTime;
            this.Direction = dir;
        }

        public void BySubmitter(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Field = jobfields.Submitter;
            this.Direction = dir;
        }

        public void ByState(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Field = jobfields.State;
            this.Direction = dir;
        }

        public void ByDegreeOfParallelism(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Field = jobfields.DegreeOfParallelism;
            this.Direction = dir;
        }

        public void Clear()
        {
            this.Field = null;
        }

    }
}
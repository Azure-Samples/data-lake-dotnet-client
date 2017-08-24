namespace AdlClient.Models
{

    public class JobListSorting
    {
        public SortSpec Spec { get; private set; }

        static JobExprFields jobfields = new JobExprFields();

        public string CreateOrderByString()
        {
            if (this.Spec == null)
            {
                return null;
            }

            return this.Spec.CreateOrderByString();
        }
        
        public void BySubmitTime(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new SortSpec( jobfields.SubmitTime, dir );
        }


        public void ByEndTime(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new SortSpec(jobfields.EndTime, dir);
        }

        public void ByName(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new SortSpec(jobfields.Name, dir);
        }

        public void ByPriority(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new SortSpec(jobfields.Priority, dir);
        }

        public void ByResult(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new SortSpec(jobfields.Result, dir);
        }

        public void ByStartTime(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new SortSpec(jobfields.StartTime, dir);
        }

        public void BySubmitter(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new SortSpec(jobfields.Submitter, dir);
        }

        public void ByState(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new SortSpec(jobfields.State, dir);
        }

        public void ByDegreeOfParallelism(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new SortSpec(jobfields.DegreeOfParallelism, dir);
        }

        public void Clear()
        {
            this.Spec = null;
        }

    }
}
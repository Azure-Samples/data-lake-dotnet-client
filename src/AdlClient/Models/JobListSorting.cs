namespace AdlClient.Models
{

    public class JobListSorting
    {
        public AdlClient.OData.Models.SortSpec Spec { get; private set; }

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
            this.Spec = new AdlClient.OData.Models.SortSpec( jobfields.SubmitTime, dir );
        }


        public void ByEndTime(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new AdlClient.OData.Models.SortSpec(jobfields.EndTime, dir);
        }

        public void ByName(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new AdlClient.OData.Models.SortSpec(jobfields.Name, dir);
        }

        public void ByPriority(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new AdlClient.OData.Models.SortSpec(jobfields.Priority, dir);
        }

        public void ByResult(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new AdlClient.OData.Models.SortSpec(jobfields.Result, dir);
        }

        public void ByStartTime(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new AdlClient.OData.Models.SortSpec(jobfields.StartTime, dir);
        }

        public void BySubmitter(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new AdlClient.OData.Models.SortSpec(jobfields.Submitter, dir);
        }

        public void ByState(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new AdlClient.OData.Models.SortSpec(jobfields.State, dir);
        }

        public void ByDegreeOfParallelism(AdlClient.OData.Models.OrderByDirection dir)
        {
            this.Spec = new AdlClient.OData.Models.SortSpec(jobfields.DegreeOfParallelism, dir);
        }

        public void Clear()
        {
            this.Spec = null;
        }
    }
}
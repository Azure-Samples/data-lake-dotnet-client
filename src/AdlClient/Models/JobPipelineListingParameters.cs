namespace AdlClient.Models
{
    public class JobPipelineListingParameters
    {
        // 100 is picked as the default number of records to 
        // retrieve so that vallers are overwhelmed with data on first use

        public int Top = 100;
        public AdlClient.Models.RangeDateTime DateRange;

        public JobPipelineListingParameters()
        {
        }
    }
}
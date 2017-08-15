namespace AdlClient.Models
{
    public class JobReccurenceListingParameters
    {
        // 100 is picked as the default number of records to 
        // retrieve so that vallers are overwhelmed with data on first use

        public int Top = 100;
        public AdlClient.OData.Models.RangeDateTime DateRange;

        public JobReccurenceListingParameters()
        {
        }
    }
}
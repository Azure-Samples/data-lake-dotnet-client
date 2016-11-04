namespace AzureDataLakeClient.OData.Utils
{
    public class RangeInteger
    {
        public readonly int? upper;
        public readonly int? lower;

        public RangeInteger(int? lower, int? upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        public bool HasUpperOrLower
        {
            get { return (this.upper.HasValue || this.lower.HasValue); }
        }
    }
}
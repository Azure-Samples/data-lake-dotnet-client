namespace AdlClient.Models
{
    public class RangeInteger
    {
        public int? UpperBound { get; }
        public int? LowerBound { get; }

        public RangeInteger(int? lower, int? upper)
        {
            this.LowerBound = lower;
            this.UpperBound = upper;
        }

        public bool IsBounded
        {
            get { return (this.UpperBound.HasValue || this.LowerBound.HasValue); }
        }
    }
}
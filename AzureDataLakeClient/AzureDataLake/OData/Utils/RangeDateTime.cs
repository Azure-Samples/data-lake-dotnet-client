namespace AzureDataLakeClient.OData.Utils
{
    public class RangeDateTime
    {
        public readonly System.DateTimeOffset? upper;
        public readonly System.DateTimeOffset? lower;

        public RangeDateTime(System.DateTimeOffset? lower, System.DateTimeOffset? upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        public static RangeDateTime InTheLastNHours(int hours)
        {
            var now = System.DateTimeOffset.UtcNow;
            var lower = now.AddHours(-hours);
            return new RangeDateTime(lower,null);
        }

        public static RangeDateTime InTheLastNDays(int days)
        {
            var now = System.DateTimeOffset.UtcNow;
            var lower = now.AddDays(-days);
            return new RangeDateTime(lower,null);
        }

        public static RangeDateTime SinceLocalMidnight()
        {
            var localnow = System.DateTime.Now;
            var localmidnight = new System.DateTime(localnow.Year, localnow.Month, localnow.Day);
            var lower = new System.DateTimeOffset(localmidnight);
            return new RangeDateTime(lower,null);
        }

        public bool HasBoundary
        {
            get { return (this.upper.HasValue || this.lower.HasValue); }
        }
    }

    public class RangeInteger
    {
        public readonly int? upper;
        public readonly int? lower;

        public RangeInteger(int? lower, int? upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        public bool HasBoundary
        {
            get { return (this.upper.HasValue || this.lower.HasValue); }
        }
    }

}
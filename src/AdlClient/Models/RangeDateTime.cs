using System;

namespace AdlClient.Models
{
    public class RangeDateTime
    {
        public DateTimeOffset? UpperBound { get; }
        public DateTimeOffset? LowerBound { get; }

        public RangeDateTime(System.DateTimeOffset? lower, System.DateTimeOffset? upper)
        {
            this.LowerBound = lower;
            this.UpperBound = upper;
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

        public bool IsBounded
        {
            get { return (this.UpperBound.HasValue || this.LowerBound.HasValue); }
        }
    }
}
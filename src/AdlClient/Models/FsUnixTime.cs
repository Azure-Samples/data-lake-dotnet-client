using System;

namespace Models
{
    public struct FsUnixTime
    {
        public readonly long MillisecondsSinceEpoch;

        public FsUnixTime(long value)
        {
            if (value < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(value));
            }
            this.MillisecondsSinceEpoch = value;
        }

        public FsUnixTime(System.DateTimeOffset time)
        {
            if (time<FsUnixTime.EpochDateTime)
            {
                throw new System.ArgumentOutOfRangeException(nameof(time));
            }

            long v = (long) time.Subtract(FsUnixTime.EpochDateTime).TotalMilliseconds;

            if (v < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(time));
            }

            this.MillisecondsSinceEpoch = v;
        }

        public static FsUnixTime UtcNow()
        {
            var ut = new FsUnixTime(System.DateTimeOffset.UtcNow);
            return ut;
        }

        public static FsUnixTime Epoch
        {
            get
            {
                var ut = new FsUnixTime(0);
                return ut;
            }
        }

        public static System.DateTimeOffset EpochDateTime
        {
            get
            {
                return new System.DateTimeOffset(1970, 1, 1,0,0,0,TimeSpan.Zero);
            }
        }

        public DateTimeOffset ToToDateTimeOffset()
        {
            var dt = FsUnixTime.EpochDateTime.AddMilliseconds(this.MillisecondsSinceEpoch);
            return dt;
        }

        public static DateTimeOffset ToToDateTimeOffset(long seconds)
        {
            var ut = new FsUnixTime(seconds);
            var dt = ut.ToToDateTimeOffset();
            return dt;
        }

        public static FsUnixTime? TryParseDouble(long? d)
        {
            if (d.HasValue)
            {
                return  new FsUnixTime(d.Value);
            }
            else
            {
                return null;
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace AdlClient.Models
{
    public class JobProfile
    {
        public System.DateTime StartTime;
        public double TimeDilation;
        public Dictionary<string, JobVertexProfile> VertexProfiles;

        private static System.DateTime? ParseDate(string dateString)
        {
            var dt = _ParseDate1(dateString);
            return dt;
        }

        private static System.DateTime? _ParseDate1(string dateString)
        {
            dateString = dateString.Replace("PDT", "-8");

            string fmt = "yyyy-MM-dd HH:mm:ss z";
            DateTime dt;
            bool result = DateTime.TryParseExact(dateString, fmt, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dt);

            return result ? (DateTime?) dt : _ParseDate2(dateString);
        }

        private static System.DateTime? _ParseDate2(string dateString)
        {
            DateTime dt;
            dateString = dateString.Replace("PDT", "-8");

            string fmt = "yyyy-MM-dd HH:mm:ss.fff z";
            bool result = DateTime.TryParseExact(dateString, fmt, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dt);

            return result ? (DateTime?)dt : null;
        }
        public static DateTime DateTimeMin(DateTime d1, DateTime d2)
        {
            return new DateTime(Math.Min(d1.Ticks, d2.Ticks));
        }

        public static DateTime DateTimeMax(DateTime d1, DateTime d2)
        {
            return new DateTime(Math.Max(d1.Ticks, d2.Ticks));
        }

        private static string FixId(string id)
        {
            if (id.IndexOf('[') != -1)
            {
                return id.Substring(0, id.IndexOf('['));
            }
            return id;
        }

        public static JobProfile Parse(string filename)
        {
            var profile = new JobProfile();

            profile.VertexProfiles = new Dictionary<string, JobVertexProfile>();

            var filetext = System.IO.File.ReadAllText(filename);

            var lines = filetext.Split('\n');
            var startTime = System.DateTime.MaxValue;
            var endTime = System.DateTime.MinValue;


            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                var cols = line.Split(',');

                if (cols[0] == "timing")
                {
                    var v = new JobVertexProfile();
                    v.ApproxEndTime = ParseDate(cols[1]);
                    v.VersionCreatedTime = ParseDate(cols[7]);
                    v.VertexCreateStart = ParseDate(cols[8]);
                    v.VertexCreateEnd = ParseDate(cols[12]);
                    v.VertexQueueStart = ParseDate(cols[19]);
                    v.VertexQueueEnd = ParseDate(cols[21]);
                    v.ApproxEndTime = ParseDate(cols[9]);
                    v.VertexPNQueueEnd = ParseDate(cols[20]);
                    v.VertexStartTime = ParseDate(cols[10]);
                    v.VertexEndTime = ParseDate(cols[22]);
                    v.CleanedUpTime = ParseDate(cols[11]);
                    v.VertexGuid = cols[3];
                    v.ProcessId = cols[4];
                    v.VertexId = FixId(cols[4]);
                    v.BytesRead = long.Parse(cols[13]);
                    v.BytesWritten = long.Parse(cols[14]);
                    v.VertexResult = cols[5];

                    if (v.VertexPNQueueEnd.HasValue && v.VersionCreatedTime.HasValue)
                    {
                        startTime = DateTimeMin(v.VersionCreatedTime.Value, startTime);
                    }

                    if (v.VertexEndTime.HasValue && v.CleanedUpTime.HasValue)
                    {
                        endTime = DateTimeMax(v.CleanedUpTime.Value, endTime);
                    }

                    profile.VertexProfiles[v.VertexGuid] = v;

                }
            }

            profile.StartTime = startTime;
            var totalTime = endTime - startTime;
            profile.TimeDilation = 1.0;//totalTime / 1.0; // Constants.JobProfile.simulationRunTime;

            return profile;
        }
    }
}
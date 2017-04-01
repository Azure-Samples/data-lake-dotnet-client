using System;
using System.Collections.Generic;

namespace AdlClient.Models
{
    public class JobProfile
    {
        public System.DateTime StartTime;
        public double TimeDilation;
        public Dictionary<string, JobVertexProfile> VertexProfiles;

        private static System.DateTime? _getDate(string dateString)
        {
            var dt = _getDate1(dateString);
            return dt;
        }

        private static System.DateTime? _getDate1(string dateString)
        {
            dateString = dateString.Replace("PDT", "-8");

            string fmt = "yyyy-MM-dd HH:mm:ss z";
            DateTime dt;
            bool result = DateTime.TryParseExact(dateString, fmt, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dt);

            return result ? (DateTime?) dt : _getDate2(dateString);
        }

        private static System.DateTime? _getDate2(string dateString)
        {
            DateTime dt;
            dateString = dateString.Replace("PDT", "-8");

            string fmt = "yyyy-MM-dd HH:mm:ss.fff z";
            bool result = DateTime.TryParseExact(dateString, fmt, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dt);

            return result ? (DateTime?)dt : null;
        }
        public static DateTime DTMin(DateTime Date1, DateTime Date2)
        {
            return new DateTime(Math.Min(Date1.Ticks, Date2.Ticks));
        }

        public static DateTime DTMax(DateTime Date1, DateTime Date2)
        {
            return new DateTime(Math.Max(Date1.Ticks, Date2.Ticks));
        }


        private static string _fixId(string id)
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
                    v.ApproxEndTime = _getDate(cols[1]);
                    v.VersionCreatedTime = _getDate(cols[7]);
                    v.VertexCreateStart = _getDate(cols[8]);
                    v.VertexCreateEnd = _getDate(cols[12]);
                    v.VertexQueueStart = _getDate(cols[19]);
                    v.VertexQueueEnd = _getDate(cols[21]);
                    v.ApproxEndTime = _getDate(cols[9]);
                    v.VertexPNQueueEnd = _getDate(cols[20]);
                    v.VertexStartTime = _getDate(cols[10]);
                    v.VertexEndTime = _getDate(cols[22]);
                    v.CleanedUpTime = _getDate(cols[11]);
                    v.VertexGuid = cols[3];
                    v.ProcessId = cols[4];
                    v.VertexId = _fixId(cols[4]);
                    v.BytesRead = long.Parse(cols[13]);
                    v.BytesWritten = long.Parse(cols[14]);
                    v.VertexResult = cols[5];

                    if (v.VertexPNQueueEnd.HasValue && v.VersionCreatedTime.HasValue)
                    {
                        startTime = DTMin(v.VersionCreatedTime.Value, startTime);
                    }

                    if (v.VertexEndTime.HasValue && v.CleanedUpTime.HasValue)
                    {
                        endTime = DTMax(v.CleanedUpTime.Value, endTime);
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
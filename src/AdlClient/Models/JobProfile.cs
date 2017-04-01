using System;
using System.Collections.Generic;

namespace AdlClient.Models
{
    public class JobProfile
    {
        public System.DateTime StartTime { get; private set; }
        public System.DateTime EndTime { get; private set; }

        public Dictionary<string, JobVertexProfile> VertexProfiles { get; private set; }

        private static System.DateTime? _parse_date(string dateString)
        {
            dateString = dateString.Replace("PDT", "-8");

            DateTime result_dt = System.DateTime.MaxValue;

            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var style = System.Globalization.DateTimeStyles.None;

            string fmt_1 = "yyyy-MM-dd HH:mm:ss z";
            string fmt_2 = "yyyy-MM-dd HH:mm:ss.fff z";

            bool dt_parsed = false;

            if (!dt_parsed)
            {
                dt_parsed = DateTime.TryParseExact(dateString, fmt_1, culture, style, out result_dt);
            }

            if (!dt_parsed)
            {
                dt_parsed = DateTime.TryParseExact(dateString, fmt_2, culture, style, out result_dt);
            }

            // If an of the parsing succeeded, then return the datetime

            if (dt_parsed)
            {
                return result_dt;
            }

            return null;
        }

        private static DateTime DateTimeMin(DateTime d1, DateTime d2)
        {
            return new DateTime(Math.Min(d1.Ticks, d2.Ticks));
        }

        private static DateTime DateTimeMax(DateTime d1, DateTime d2)
        {
            return new DateTime(Math.Max(d1.Ticks, d2.Ticks));
        }

        private static string _get_vertex_id(string id)
        {
            if (id.IndexOf('[') != -1)
            {
                return id.Substring(0, id.IndexOf('['));
            }
            return id;
        }

        public static JobProfile LoadProfile(string filename)
        {
            var profile = new JobProfile();

            profile.VertexProfiles = new Dictionary<string, JobVertexProfile>();

            var filetext = System.IO.File.ReadAllText(filename);

            var lines = filetext.Split('\n');
            profile.StartTime = System.DateTime.MaxValue;
            profile.EndTime = System.DateTime.MinValue;

            foreach (string line in lines)
            {
                var cols = line.Split(',');

                if (cols[0] == "timing")
                {
                    var v = get_vertex_info(cols);

                    // Update profile StartTime if needed
                    if (v.PNQueueEnd.HasValue && v.CreatedTime.HasValue)
                    {
                        profile.StartTime = DateTimeMin(v.CreatedTime.Value, profile.StartTime);
                    }

                    // Update profile EndTime if needed
                    if (v.EndTime.HasValue && v.CleanedUpTime.HasValue)
                    {
                        profile.EndTime = DateTimeMax(v.CleanedUpTime.Value, profile.EndTime);
                    }

                    // Store (or update) the verex information
                    profile.VertexProfiles[v.VertexGuid] = v;
                }
            }

            return profile;
        }

        private static JobVertexProfile get_vertex_info(string[] cols)
        {
            var v = new JobVertexProfile();
            v.ApproxEndTime = _parse_date(cols[1]);
            v.CreatedTime = _parse_date(cols[7]);

            v.CreateStart = _parse_date(cols[8]);
            v.CreateEnd = _parse_date(cols[12]);

            v.QueueStart = _parse_date(cols[19]);
            v.QueueEnd = _parse_date(cols[21]);

            v.PNQueueStart = _parse_date(cols[9]);
            v.PNQueueEnd = _parse_date(cols[20]);

            v.StartTime = _parse_date(cols[10]);
            v.EndTime = _parse_date(cols[22]);

            v.CleanedUpTime = _parse_date(cols[11]);

            v.VertexGuid = cols[3];
            v.ProcessId = cols[4];
            v.VertexId = _get_vertex_id(cols[4]);
            v.VertexResult = cols[5];

            v.BytesRead = long.Parse(cols[13]);
            v.BytesWritten = long.Parse(cols[14]);

            return v;
        }
    }
}
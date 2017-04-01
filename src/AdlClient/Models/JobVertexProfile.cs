namespace AdlClient.Models
{
    public class JobVertexProfile
    {
        public System.DateTime? CreatedTime;
        public System.DateTime? CreateStart;
        public System.DateTime? CreateEnd;

        public System.DateTime? QueueStart;
        public System.DateTime? QueueEnd;

        public System.DateTime? PNQueueStart;
        public System.DateTime? PNQueueEnd;

        public System.DateTime? StartTime;
        public System.DateTime? EndTime;

        public System.DateTime? CleanedUpTime;

        public System.DateTime? ApproxEndTime;

        public string VertexGuid;
        public string ProcessId;
        public string VertexId;

        public long BytesRead;
        public long BytesWritten;

        public string VertexResult;
    }
}
namespace AdlClient.Models
{
    public class JobVertexProfile
    {
        public System.DateTime? ApproxEndTime;
        public System.DateTime? VersionCreatedTime;
        public System.DateTime? VertexCreateStart;
        public System.DateTime? VertexCreateEnd;
        public System.DateTime? VertexQueueStart;
        public System.DateTime? VertexQueueEnd;
        public System.DateTime? VertexPNQueueStart;
        public System.DateTime? VertexPNQueueEnd;
        public System.DateTime? VertexStartTime;
        public System.DateTime? VertexEndTime;
        public System.DateTime? CleanedUpTime;
        public string VertexGuid;
        public string ProcessId;
        public string VertexId;
        public long BytesRead;
        public long BytesWritten;
        public string VertexResult;
    }
}
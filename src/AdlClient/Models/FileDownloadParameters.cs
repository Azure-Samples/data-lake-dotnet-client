namespace AdlClient.Models
{
    public class FileDownloadParameters
    {
        public bool Overwrite = false;
        public int NumThreads = -1;
        public bool Resume = false;
    }
}
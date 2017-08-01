namespace AdlClient.Models
{
    public class FileUploadParameters
    {
        public bool Overwrite = false;
        public int NumThreads = -1;
        public bool Resume = false;
        public bool UploadAsBinary = false;
    }
}
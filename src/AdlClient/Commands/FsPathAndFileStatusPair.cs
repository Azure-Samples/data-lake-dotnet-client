namespace AdlClient.Models
{
    public struct FsPathAndFileStatusPair
    {
        public readonly FsPath FsPath;
        public readonly FsFileStatus FsFileStatus;

        public FsPathAndFileStatusPair(FsPath path, FsFileStatus status)
        {
            this.FsPath = path;
            this.FsFileStatus = status;
        }
    }
}
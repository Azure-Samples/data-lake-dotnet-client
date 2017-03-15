using System.Collections.Generic;

namespace AdlClient.FileSystem
{
    public class FsFileStatusPage
    {
        public FsPath Path;
        public IList<FsFileStatus> FileItems;
    }
}
using System.Collections.Generic;

namespace AdlClient.Models
{
    public class FsFileStatusPage
    {
        public FsPath Path;
        public IList<FsFileStatus> FileItems;
    }
}
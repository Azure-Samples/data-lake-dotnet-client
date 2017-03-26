using System.Collections.Generic;

namespace Models
{
    public class FsFileStatusPage
    {
        public FsPath Path;
        public IList<FsFileStatus> FileItems;
    }
}
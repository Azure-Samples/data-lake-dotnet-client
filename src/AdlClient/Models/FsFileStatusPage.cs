using System.Collections.Generic;

namespace AdlClient.Models
{
    public class FsFileStatusPage
    {
        public readonly FsPath Path;
        public readonly IList<FsFileStatus> FileItems;

        public FsFileStatusPage(FsPath path, IList<FsFileStatus> items)
        {
            this.Path = path;
            this.FileItems = items;
        }
    }
}
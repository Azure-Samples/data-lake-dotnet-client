using System.Collections.Generic;

namespace AzureDataLakeClient.FileSystem
{
    public class FsFileStatusPage
    {
        public FsPath Path;
        public IList<FsFileStatus> FileItems;
    }
}
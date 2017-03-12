using System.Collections.Generic;

namespace AzureDataLakeClient.Store.FileSystem
{
    public class FsFileStatusPage
    {
        public FsPath Path;
        public IList<FsFileStatus> FileItems;
    }
}
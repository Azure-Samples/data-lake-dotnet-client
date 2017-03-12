using System.Collections.Generic;

namespace AzureDataLakeClient.Store
{
    public class FsFileStatusPage
    {
        public FsPath Path;
        public IList<FsFileStatus> FileItems;
    }
}
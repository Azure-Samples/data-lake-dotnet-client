namespace AdlClient.Models
{
    public class FsPathUri
    {
        public readonly string Account;
        public readonly string Path;

        public FsPathUri(string account, string path)
        {
            this.Account = account;
            this.Path = path;
        }
    }
}
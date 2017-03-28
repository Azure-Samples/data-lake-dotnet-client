namespace AdlClient.Models
{
    public class FolderAzurePortalUri
    {

        public readonly string Account;
        public readonly string Path;

        public FolderAzurePortalUri(string account, string path)
        {
            this.Account = account;
            this.Path = path;
        }

        public static FolderAzurePortalUri Parse(string s)
        {
            // https://portal.azure.com/#blade/Microsoft_Azure_DataLakeStore/WebHdfsFolderBlade/endpoint/adlpm.azuredatalakestore.net/path/%2F

            var uri = new System.Uri(s);

            var adl_portal_authority = "portal.azure.com";
            if (uri.Authority.ToLower() != adl_portal_authority)
            {
                string msg = string.Format("Malformed job portal uri: Uri.Authority must be with \"{0}\"", adl_portal_authority);
                throw new System.ArgumentException(msg);
            }

            string fragment = uri.Fragment;

            string fragment_prefix = "#blade/Microsoft_Azure_DataLakeStore/WebHdfsFolderBlade/endpoint/";

            if (!fragment.StartsWith(fragment_prefix))
            {
                throw new System.ArgumentException("invalid uri fragment");
            }

            var fragment_remainder = fragment.Substring(fragment_prefix.Length);

            var fragment_tokens = fragment_remainder.Split('/');

            // Basic fragment validation

            if (fragment_tokens.Length < 3)
            {
                throw new System.ArgumentException("Malformed job portal uri: has Fragment has less than 3 parts");
            }

            // Subscription
            if (!fragment_tokens[0].EndsWith("azuredatalakestore.net"))
            {

                throw new System.ArgumentException(
                    "Malformed job portal uri: Fragment at position 0 does not end with \"azuredatalakestore.net\"");
            }

            var t2 = fragment_tokens[0].Split('.');
            string account_name = t2[0];

            // Provider
            if (fragment_tokens[1] != "path")
            {
                throw new System.ArgumentException(
                    "Malformed job portal uri: missing \"path\" in Fragment at position 1");
            }

            string path = fragment_tokens[2];
            path = System.Uri.UnescapeDataString(path);
            // Final URI
            var portal_uri = new FolderAzurePortalUri(account_name, path);
            return portal_uri;
        }

        public override string ToString()
        {
            string escaped_path = System.Uri.EscapeDataString(this.Path);
            string uri =
                string.Format(
                    "https://portal.azure.com/#blade/Microsoft_Azure_DataLakeStore/WebHdfsFolderBlade/endpoint/{0}.azuredatalakestore.net/path/{1}",
                    this.Account,
                    escaped_path
                    );
            return uri;
        }
    }
}
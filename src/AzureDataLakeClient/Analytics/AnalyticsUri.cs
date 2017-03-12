namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsAccountUri
    {
        public readonly string Name;

        public AnalyticsAccountUri(string name)
        {
            this.Name = name;
        }

        public System.Uri GetUri()
        {
            return new System.Uri("https://" + this.Name + "." + "azuredatalakeanalytics.net");
        }
    }
}
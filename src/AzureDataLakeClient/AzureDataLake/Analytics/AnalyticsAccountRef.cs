namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsAccountRef
    {
        public string Name;
        public string ResourceGroup;

        public AnalyticsAccountRef(string name, string rg)
        {
            this.Name = name;
            this.ResourceGroup = rg;
        }
    }
}
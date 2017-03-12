namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsAccountRmRef
    {
        public string Name;
        public AzureDataLakeClient.Rm.ResourceGroup ResourceGroup;

        public AnalyticsAccountRmRef(string name, AzureDataLakeClient.Rm.ResourceGroup rg)
        {
            this.Name = name;
            this.ResourceGroup = rg;
        }
    }
}
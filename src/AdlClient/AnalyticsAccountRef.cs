namespace AdlClient
{
    public class AnalyticsAccountRef
    {
        public string SubscriptionId;
        public string ResourceGroup;
        public string Name;

        public AnalyticsAccountRef(string sub, string rg, string name)
        {
            this.Name = name;
            this.SubscriptionId = sub;
            this.ResourceGroup = rg;
        }
    }
}
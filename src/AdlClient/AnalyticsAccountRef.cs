namespace AdlClient
{
    public class AnalyticsAccountRef
    {
        public string SubscriptionId;
        public string ResourceGroup;
        public string Name;

        public AnalyticsAccountRef(string subid, string rg, string name)
        {
            this.SubscriptionId = subid;
            this.ResourceGroup = rg;
            this.Name = name;
        }
    }
}
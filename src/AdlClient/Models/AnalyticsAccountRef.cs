namespace AdlClient.Models
{
    public class AnalyticsAccountRef
    {
        public readonly string SubscriptionId;
        public readonly string ResourceGroup;
        public readonly string Name;

        public AnalyticsAccountRef(string subid, string rg, string name)
        {
            this.SubscriptionId = subid;
            this.ResourceGroup = rg;
            this.Name = name;
        }
    }
}
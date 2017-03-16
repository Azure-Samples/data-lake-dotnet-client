namespace AdlClient
{
    public class AnalyticsAccount
    {
        public string Name;
        public string SubscriptionId;
        public string ResourceGroup;

        public AnalyticsAccount(string sub, string rg, string name)
        {
            this.Name = name;
            this.SubscriptionId = sub;
            this.ResourceGroup = rg;
        }
    }
}
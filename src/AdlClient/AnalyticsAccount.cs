namespace AdlClient
{
    public class AnalyticsAccount
    {
        public string Name;
        public Subscription Subscription;
        public ResourceGroup ResourceGroup;

        public AnalyticsAccount(Subscription sub, ResourceGroup rg, string name)
        {
            this.Name = name;
            this.Subscription = sub;
            this.ResourceGroup = rg;
        }
    }
}
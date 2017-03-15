namespace AdlClient
{
    public class AnalyticsAccount
    {
        public string Name;
        public Subscription Subscription;
        public ResourceGroup ResourceGroup;

        public AnalyticsAccount(string name, Subscription sub, ResourceGroup rg)
        {
            this.Name = name;
            this.Subscription = sub;
            this.ResourceGroup = rg;
        }
    }
}
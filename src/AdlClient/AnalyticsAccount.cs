namespace AdlClient
{
    public class AnalyticsAccount
    {
        public string Name;
        public string Subscription;
        public string ResourceGroup;

        public AnalyticsAccount(string sub, string rg, string name)
        {
            this.Name = name;
            this.Subscription = sub;
            this.ResourceGroup = rg;
        }
    }
}
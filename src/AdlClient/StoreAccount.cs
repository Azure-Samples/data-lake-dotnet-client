namespace AdlClient
{
    public class StoreAccount
    {
        public readonly string SubscriptionId;
        public readonly string ResourceGroup;
        public readonly string Name;

        public StoreAccount(string sub, string rg, string name)
        {
            this.Name = name;
            this.SubscriptionId = sub;
            this.ResourceGroup = rg;
        }
    }
}


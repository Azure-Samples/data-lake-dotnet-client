namespace AdlClient
{
    public class StoreAccountRef
    {
        public readonly string SubscriptionId;
        public readonly string ResourceGroup;
        public readonly string Name;

        public StoreAccountRef(string sub, string rg, string name)
        {
            this.Name = name;
            this.SubscriptionId = sub;
            this.ResourceGroup = rg;
        }
    }
}


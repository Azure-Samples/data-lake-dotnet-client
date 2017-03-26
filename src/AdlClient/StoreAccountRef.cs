namespace AdlClient
{
    public class StoreAccountRef
    {
        public readonly string SubscriptionId;
        public readonly string ResourceGroup;
        public readonly string Name;

        public StoreAccountRef(string Id, string rg, string name)
        {
            this.Name = name;
            this.SubscriptionId = Id;
            this.ResourceGroup = rg;
        }
    }
}


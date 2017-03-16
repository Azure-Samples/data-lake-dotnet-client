namespace AdlClient
{
    public class StoreAccount
    {
        public readonly string Name;
        public readonly Subscription Subscription;
        public readonly ResourceGroup ResourceGroup;

        public StoreAccount(Subscription sub, ResourceGroup rg, string name)
        {
            this.Name = name;
            this.Subscription = sub;
            this.ResourceGroup = rg;
        }
    }
}


namespace AdlClient
{
    public class StoreAccount
    {
        public string Name;
        public Subscription Subscription;
        public ResourceGroup ResourceGroup;

        public StoreAccount(Subscription sub, ResourceGroup rg, string name)
        {
            this.Name = name;
            this.Subscription = sub;
            this.ResourceGroup = rg;
        }
    }
}


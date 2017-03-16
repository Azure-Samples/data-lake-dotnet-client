namespace AdlClient
{
    public class StoreAccount
    {
        public readonly string Subscription;
        public readonly string ResourceGroup;
        public readonly string Name;

        public StoreAccount(string sub, string rg, string name)
        {
            this.Name = name;
            this.Subscription = sub;
            this.ResourceGroup = rg;
        }
    }
}


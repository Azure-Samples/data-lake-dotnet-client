namespace AzureDataLakeClient
{
    public class StoreAccount
    {
        public string Name;
        public Subscription Subscription;
        public ResourceGroup ResourceGroup;

        public StoreAccount(string name, Subscription sub, ResourceGroup rg)
        {
            this.Name = name;
            this.Subscription = sub;
            this.ResourceGroup = rg;
        }
    }
}


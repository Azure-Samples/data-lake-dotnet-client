namespace AzureDataLakeClient.Store
{
    public class StoreAccount
    {
        public string Name;
        public AzureDataLakeClient.Rm.Subscription Subscription;
        public AzureDataLakeClient.Rm.ResourceGroup ResourceGroup;

        public StoreAccount(string name, AzureDataLakeClient.Rm.Subscription sub, AzureDataLakeClient.Rm.ResourceGroup rg)
        {
            this.Name = name;
            this.Subscription = sub;
            this.ResourceGroup = rg;
        }

        public StoreUri GetUri()
        {
            return new StoreUri(this.Name);
        }
    }
}


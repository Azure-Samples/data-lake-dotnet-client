namespace AzureDataLakeClient.Store
{
    public class StoreAccountRef
    {
        public string Name;
        public string ResourceGroup;

        public StoreAccountRef(string name, string rg)
        {
            this.Name = name;
            this.ResourceGroup = rg;
        }
    }

    public class StoreUri
    {
        public readonly string Name;

        public StoreUri(string name)
        {
            this.Name = name;
        }
    }
}
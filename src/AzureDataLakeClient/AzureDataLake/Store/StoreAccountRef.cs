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
}
namespace AzureDataLakeClient.Store
{
    public class StoreAccountRmRef
    {
        public string Name;
        public string ResourceGroup;

        public StoreAccountRmRef(string name, string rg)
        {
            this.Name = name;
            this.ResourceGroup = rg;
        }
    }
}
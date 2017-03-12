namespace AzureDataLakeClient.Store
{
    public class StoreUri
    {
        public readonly string Name;

        public StoreUri(string name)
        {
            this.Name = name;
        }

        public System.Uri GetUri()
        {
            return new System.Uri("https://" + this.Name + "." + "azuredatalakestore.net");
        }

    }
}
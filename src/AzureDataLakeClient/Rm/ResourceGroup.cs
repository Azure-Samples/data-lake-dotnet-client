namespace AzureDataLakeClient.Rm
{
    public class ResourceGroup
    {
        public readonly string Name;

        public ResourceGroup(string name)
        {
            this.Name = name;
        }
    }
}
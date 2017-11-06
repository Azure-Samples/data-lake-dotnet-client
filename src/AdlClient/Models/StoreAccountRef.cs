namespace AdlClient.Models
{
    public class StoreAccountRef
    {
        public readonly string SubscriptionId;
        public readonly string ResourceGroup;
        public readonly string Name;

        public StoreAccountRef(string subid, string rg, string name)
        {
            this.Name = name;
            this.SubscriptionId = subid;
            this.ResourceGroup = rg;
        }
    }
}


namespace AzureDataLakeClient.Authentication
{
    public class Tenant
    {
        public readonly string Domain;

        public Tenant(string domain)
        {
            this.Domain = domain;
        }
    }
}
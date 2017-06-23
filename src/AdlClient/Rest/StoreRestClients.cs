namespace AdlClient.Rest
{
    public class StoreRestClients
    {
        public readonly StoreFileSystemRestWrapper FileSystemRest;
        public readonly StoreManagementRestWrapper StoreAccountMgmtRest;
        public readonly AdlClient.Models.StoreAccountRef Account;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient GraphClient;

        public StoreRestClients(Authentication authSession, AdlClient.Models.StoreAccountRef account)
        {
            this.Account = account;
            this.FileSystemRest = new StoreFileSystemRestWrapper(authSession.ADLCreds);
            this.StoreAccountMgmtRest = new StoreManagementRestWrapper(account.SubscriptionId, authSession.ARMCreds);
            this.GraphClient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(authSession.AADCreds);
            this.GraphClient.TenantID = authSession.Tenant;
        }
    }
}
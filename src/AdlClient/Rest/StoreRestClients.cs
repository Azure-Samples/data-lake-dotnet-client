namespace AdlClient.Rest
{
    public class StoreRestClients
    {
        public readonly StoreFileSystemRestWrapper FileSystemRest;
        public readonly StoreManagementRestWrapper StoreAccountMgmtRest;
        public readonly AdlClient.Models.StoreAccountRef Account;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient AADclient;

        public StoreRestClients(Authentication authSession, AdlClient.Models.StoreAccountRef account)
        {
            this.Account = account;
            this.FileSystemRest = new StoreFileSystemRestWrapper(authSession.ADLCreds);
            this.StoreAccountMgmtRest = new StoreManagementRestWrapper(account.SubscriptionId, authSession.ARMCreds);
            this.AADclient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(authSession.AADCreds);
            this.AADclient.TenantID = authSession.Tenant;
        }
    }
}
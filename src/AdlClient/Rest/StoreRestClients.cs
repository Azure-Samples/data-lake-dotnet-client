namespace AdlClient.Rest
{
    public class StoreRestClients
    {
        public readonly StoreFileSystemRestWrapper FileSystemRest;
        public readonly StoreManagementRestWrapper StoreAccountMgmtRest;
        public readonly AdlClient.Models.StoreAccountRef Account;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient AADclient;

        public StoreRestClients(InteractiveAuthentication authSession, AdlClient.Models.StoreAccountRef account)
        {
            this.Account = account;
            this.FileSystemRest = new StoreFileSystemRestWrapper(authSession.AdlCreds);
            this.StoreAccountMgmtRest = new StoreManagementRestWrapper(account.SubscriptionId, authSession.ArmCreds);
            this.AADclient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(authSession.AadCreds);
            this.AADclient.TenantID = authSession.Tenant;
        }
    }
}
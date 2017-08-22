using Microsoft.Azure.Management.DataLake.Store;
using ADL = Microsoft.Azure.Management.DataLake;
namespace AdlClient.Rest
{
    public class StoreRestClients
    {
        public readonly ADL.Store.DataLakeStoreFileSystemManagementClient FileSystem;
        public readonly StoreManagementRestWrapper StoreAccountMgmtRest;
        public readonly AdlClient.Models.StoreAccountRef Account;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient AADclient;

        public StoreRestClients(Authentication authSession, AdlClient.Models.StoreAccountRef account)
        {
            this.Account = account;
            this.StoreAccountMgmtRest = new StoreManagementRestWrapper(account.SubscriptionId, authSession.ARMCreds);
            this.AADclient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(authSession.AADCreds);
            this.AADclient.TenantID = authSession.Tenant;

            this.FileSystem = new DataLakeStoreFileSystemManagementClient(authSession.ADLCreds);
        }
    }
}
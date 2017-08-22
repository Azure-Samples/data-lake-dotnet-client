using Microsoft.Azure.Management.DataLake.Store;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Rest
{
    public class StoreRestClients
    {
        public readonly ADL.Store.DataLakeStoreFileSystemManagementClient FileSystem;
        public readonly AdlClient.Models.StoreAccountRef Account;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient AADclient;
        public readonly MSADLS.DataLakeStoreAccountManagementClient AccountClient;

        public StoreRestClients(Authentication authSession, AdlClient.Models.StoreAccountRef account)
        {
            this.Account = account;
            this.AADclient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(authSession.AADCreds);
            this.AADclient.TenantID = authSession.Tenant;

            this.FileSystem = new DataLakeStoreFileSystemManagementClient(authSession.ADLCreds);
            this.AccountClient = new DataLakeStoreAccountManagementClient(authSession.ARMCreds);
            this.AccountClient.SubscriptionId = account.SubscriptionId;
        }
    }
}
using Microsoft.Azure.Management.DataLake.Store;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Rest
{
    public class StoreRestClients
    {
        public readonly ADL.Store.DataLakeStoreFileSystemManagementClient FileSystemClient;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient GraphClient;
        public readonly MSADLS.DataLakeStoreAccountManagementClient AccountClient;

        public readonly AdlClient.Models.StoreAccountRef Account;

        public StoreRestClients(Authentication authSession, AdlClient.Models.StoreAccountRef account)
        {
            this.Account = account;
            this.GraphClient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(authSession.AADCreds);
            this.GraphClient.TenantID = authSession.Tenant;

            this.FileSystemClient = new DataLakeStoreFileSystemManagementClient(authSession.ADLCreds);
            this.AccountClient = new DataLakeStoreAccountManagementClient(authSession.ARMCreds);
            this.AccountClient.SubscriptionId = account.SubscriptionId;
        }
    }
}
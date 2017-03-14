using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.FileSystem;
using AzureDataLakeClient.Rest;

namespace AzureDataLakeClient
{
    public class StoreRestClients
    {
        public readonly StoreFileSystemRestWrapper FileSystemRest;
        public readonly StoreManagementRestWrapper StoreAccountMgmtRest;

        public StoreRestClients(StoreAccount store, AuthenticatedSession authSession)
        {
            this.FileSystemRest = new StoreFileSystemRestWrapper(authSession.Credentials);
            this.StoreAccountMgmtRest = new StoreManagementRestWrapper(store.Subscription, authSession.Credentials);
        }
    }

    public class StoreClient : ClientBase
    {
        public readonly StoreRestClients RestClients;
        public readonly StoreAccount Store;
        public readonly FileSystemCommands FileSystem;

        public StoreClient(StoreAccount store, AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Store = store;
            this.RestClients = new StoreRestClients(store, authSession);
            this.FileSystem = new FileSystemCommands(this.Store, RestClients);
        }
    }
}
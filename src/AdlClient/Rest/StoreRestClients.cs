using AdlClient.Rest;

namespace AdlClient
{
    public class StoreRestClients
    {
        public readonly StoreFileSystemRestWrapper FileSystemRest;
        public readonly StoreManagementRestWrapper StoreAccountMgmtRest;
        public readonly StoreAccount Store;

        public StoreRestClients(StoreAccount store, Authentication authSession)
        {
            this.Store = store;
            this.FileSystemRest = new StoreFileSystemRestWrapper(authSession.Credentials);
            this.StoreAccountMgmtRest = new StoreManagementRestWrapper(store.SubscriptionId, authSession.Credentials);
        }
    }
}
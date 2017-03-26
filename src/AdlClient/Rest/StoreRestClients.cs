namespace AdlClient.Rest
{
    public class StoreRestClients
    {
        public readonly StoreFileSystemRestWrapper FileSystemRest;
        public readonly StoreManagementRestWrapper StoreAccountMgmtRest;
        public readonly StoreAccountRef Store;

        public StoreRestClients(Authentication authSession, StoreAccountRef store)
        {
            this.Store = store;
            this.FileSystemRest = new StoreFileSystemRestWrapper(authSession.Credentials);
            this.StoreAccountMgmtRest = new StoreManagementRestWrapper(store.SubscriptionId, authSession.Credentials);
        }
    }
}
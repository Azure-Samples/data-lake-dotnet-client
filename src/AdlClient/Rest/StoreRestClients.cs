namespace AdlClient.Rest
{
    public class StoreRestClients
    {
        public readonly StoreFileSystemRestWrapper FileSystemRest;
        public readonly StoreManagementRestWrapper StoreAccountMgmtRest;
        public readonly AdlClient.Models.StoreAccountRef Account;

        public StoreRestClients(Authentication authSession, AdlClient.Models.StoreAccountRef account)
        {
            this.Account = account;
            this.FileSystemRest = new StoreFileSystemRestWrapper(authSession.ADLCreds);
            this.StoreAccountMgmtRest = new StoreManagementRestWrapper(account.SubscriptionId, authSession.ADLCreds);
        }
    }
}
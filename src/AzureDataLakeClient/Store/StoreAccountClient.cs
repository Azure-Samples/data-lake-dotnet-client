using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;
using AzureDataLakeClient.Store.FileSystem;

namespace AzureDataLakeClient.Store
{
    public class StoreAccountClient : AccountClientBase
    {
        private StoreFileSystemRestWrapper _FileSystemRest;
        private StoreManagementRestWrapper _StoreAccountMgmtRest;
        private StoreAccount _StoreAccount;

        public readonly FileSystemCommands FileSystem;

        public StoreAccountClient(StoreAccount store, AuthenticatedSession authSession) :
            base(store.Name,authSession)
        {
            this._StoreAccount = store;
            this._FileSystemRest  = new StoreFileSystemRestWrapper(authSession.Credentials);
            this.FileSystem = new FileSystemCommands(this._StoreAccount, this._FileSystemRest);

        }

    }
}
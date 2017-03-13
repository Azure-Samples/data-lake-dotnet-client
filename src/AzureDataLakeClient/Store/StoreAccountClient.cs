using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;
using AzureDataLakeClient.Store.FileSystem;

namespace AzureDataLakeClient.Store
{
    public class StoreAccountClient : AccountClientBase
    {
        private StoreFileSystemRestWrapper _fsRestClientWrapper;
        private StoreManagementRestWrapper _mgmt_rest_client;
        private StoreAccount _store;

        public readonly FileSystemCommands FileSystem;

        public StoreAccountClient(StoreAccount store, AuthenticatedSession authSession) :
            base(store.Name,authSession)
        {
            this._store = store;
            this._fsRestClientWrapper  = new StoreFileSystemRestWrapper(authSession.Credentials);
            this.FileSystem = new FileSystemCommands(this._store, this._fsRestClientWrapper);

        }

    }
}
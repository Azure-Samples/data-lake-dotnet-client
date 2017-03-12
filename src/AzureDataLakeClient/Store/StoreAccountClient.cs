using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Store.Clients;

namespace AzureDataLakeClient.Store
{
    public class StoreAccountClient : AccountClientBase
    {
        private StoreFileSystemRestClient _fs_rest_client;
        private StoreManagementRestClient _mgmt_rest_client;
        private StoreAccount _store;

        public readonly FileSystemCommands FileSystem;

        public StoreAccountClient(StoreAccount store, AuthenticatedSession authSession) :
            base(store.Name,authSession)
        {
            this._store = store;
            this._fs_rest_client  = new StoreFileSystemRestClient(authSession.Credentials);
            this.FileSystem = new FileSystemCommands(this._store, this._fs_rest_client);

        }

    }
}
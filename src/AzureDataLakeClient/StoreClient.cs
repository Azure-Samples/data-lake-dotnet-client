using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.FileSystem;
using AzureDataLakeClient.Rest;

namespace AzureDataLakeClient
{
    public class StoreClient : ClientBase
    {
        private readonly StoreFileSystemRestWrapper _FileSystemRest;
        private readonly StoreManagementRestWrapper _StoreAccountMgmtRest;

        private StoreAccount _StoreAccount;

        public readonly FileSystemCommands FileSystem;

        public StoreClient(StoreAccount store, AuthenticatedSession authSession) :
            base(authSession)
        {
            this._StoreAccount = store;
            this._FileSystemRest  = new StoreFileSystemRestWrapper(authSession.Credentials);
            this.FileSystem = new FileSystemCommands(this._StoreAccount, this._FileSystemRest);

        }

    }
}
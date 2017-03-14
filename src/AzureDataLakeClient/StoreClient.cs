using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.FileSystem;

namespace AzureDataLakeClient
{
    public class StoreClient : ClientBase
    {
        public readonly StoreRestClients RestClients;
        public readonly FileSystemCommands FileSystem;

        public StoreClient(StoreAccount store, AuthenticatedSession authSession) :
            base(authSession)
        {
            this.RestClients = new StoreRestClients(store, authSession);
            this.FileSystem = new FileSystemCommands(store, RestClients);
        }
    }
}
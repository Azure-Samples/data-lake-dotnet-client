using AdlClient.Commands;

namespace AdlClient
{
    public class StoreClient : ClientBase
    {
        public readonly StoreRestClients RestClients;
        public readonly FileSystemCommands FileSystem;

        public StoreClient(Authentication auth, StoreAccountRef store) :
            base(auth)
        {
            this.RestClients = new StoreRestClients(auth, store);
            this.FileSystem = new FileSystemCommands(store, RestClients);
        }
    }
}
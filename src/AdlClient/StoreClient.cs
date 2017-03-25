using AdlClient.Commands;

namespace AdlClient
{
    public class StoreClient : ClientBase
    {
        public readonly StoreRestClients RestClients;
        public readonly FileSystemCommands FileSystem;
        public readonly StoreAccountCommands Account;

        public StoreClient(Authentication auth, StoreAccountRef store) :
            base(auth)
        {
            this.RestClients = new StoreRestClients(auth, store);
            this.FileSystem = new FileSystemCommands(store, this.RestClients);
            this.Account = new StoreAccountCommands(store, this.RestClients);
        }
    }
}

namespace AdlClient.Commands
{
}
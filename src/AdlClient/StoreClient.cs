namespace AdlClient
{
    public class StoreClient : ClientBase
    {
        public readonly AdlClient.Rest.StoreRestClients RestClients;
        public readonly AdlClient.Commands.FileSystemCommands FileSystem;
        public readonly AdlClient.Commands.StoreAccountCommands Account;

        public StoreClient(Authentication auth, AdlClient.Models.StoreAccountRef store) :
            base(auth)
        {
            this.RestClients = new AdlClient.Rest.StoreRestClients(auth, store);
            this.FileSystem = new AdlClient.Commands.FileSystemCommands(store, this.RestClients);
            this.Account = new AdlClient.Commands.StoreAccountCommands(store, this.RestClients);
        }
    }
}
namespace AdlClient
{
    public class StoreClient : ClientBase
    {
        public readonly AdlClient.Rest.StoreRestClients RestClients;
        public readonly AdlClient.Commands.FileSystemCommands FileSystem;
        public readonly AdlClient.Commands.StoreAccountCommands Account;

        public StoreClient(Authentication auth, AdlClient.Models.StoreAccountRef account) :
            base(auth)
        {
            this.RestClients = new AdlClient.Rest.StoreRestClients(auth, account);
            this.FileSystem = new AdlClient.Commands.FileSystemCommands(account, this.RestClients);
            this.Account = new AdlClient.Commands.StoreAccountCommands(account, this.RestClients);
        }
    }
}
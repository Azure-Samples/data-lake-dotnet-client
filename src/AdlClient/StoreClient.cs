namespace AdlClient
{
    public class StoreClient : ClientBase
    {
        public readonly StoreRestClients RestClients;
        public readonly AdlClient.FileSystem.FileSystemCommands FileSystem;

        public StoreClient(StoreAccount store, AdlClient.Authentication.AuthenticatedSession authSession) :
            base(authSession)
        {
            this.RestClients = new StoreRestClients(store, authSession);
            this.FileSystem = new AdlClient.FileSystem.FileSystemCommands(store, RestClients);
        }
    }
}
using AdlClient.Authentication;
using AdlClient.Catalog;
using AdlClient.Jobs;

namespace AdlClient
{
    public class AnalyticsClient : ClientBase
    {
        public readonly JobCommands Jobs;
        public readonly CatalogCommands Catalog;
        public readonly ManagementCommands Management;

        public AnalyticsRestClients RestClients;

        public AnalyticsClient(AnalyticsAccount account, AuthenticatedSession auth) :
            base(auth)
        {
            this.RestClients = new AnalyticsRestClients(account, auth);

            this.Jobs = new JobCommands(account, this.RestClients);
            this.Catalog = new CatalogCommands(account, this.RestClients);
            this.Management = new ManagementCommands(account, this.RestClients);
        }        
    }
}
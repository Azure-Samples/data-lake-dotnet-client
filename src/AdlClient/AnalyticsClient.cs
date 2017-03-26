using AdlClient.Commands;
using AdlClient.Models;

namespace AdlClient
{
    public class AnalyticsClient : ClientBase
    {
        public readonly JobCommands Jobs;
        public readonly CatalogCommands Catalog;
        public readonly AnalyticsAccountCommands Account;

        public AnalyticsRestClients RestClients;

        public AnalyticsClient(Authentication auth, AdlClient.Models.AnalyticsAccountRef account) :
            base(auth)
        {
            this.RestClients = new AnalyticsRestClients(auth, account);

            this.Jobs = new JobCommands(account, this.RestClients);
            this.Catalog = new CatalogCommands(account, this.RestClients);
            this.Account = new AnalyticsAccountCommands(account, this.RestClients);
        }        
    }
}
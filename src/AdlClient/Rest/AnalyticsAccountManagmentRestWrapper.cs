using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Rest.Azure;
// have to have this using clause to get the extension methods
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Rest
{
    public class AnalyticsAccountManagmentRestWrapper
    {
        public MSADLA.DataLakeAnalyticsAccountManagementClient RestClient;

        public AnalyticsAccountManagmentRestWrapper(string sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.RestClient = new MSADLA.DataLakeAnalyticsAccountManagementClient(creds);
            this.RestClient.SubscriptionId = sub;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var pageiter = new PagedIterator<MSADLA.Models.DataLakeAnalyticsAccount>();
            pageiter.GetFirstPage = () => this.RestClient.Account.List();
            pageiter.GetNextPage = p => this.RestClient.Account.ListNext(p.NextPageLink);

            int top = 0;

            var accounts = pageiter.EnumerateItems(top);

            return accounts;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccounts(string rg)
        {
            var pageiter = new PagedIterator<MSADLA.Models.DataLakeAnalyticsAccount>();
            pageiter.GetFirstPage = () => this.RestClient.Account.ListByResourceGroup(rg);
            pageiter.GetNextPage = p => this.RestClient.Account.ListByResourceGroupNext(p.NextPageLink);

            int top = 0;

            var accounts = pageiter.EnumerateItems(top);

            return accounts;
        }

        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(AdlClient.Models.AnalyticsAccountRef account)
        {
            var adls_account = this.RestClient.Account.Get(account.ResourceGroup, account.Name);
            return adls_account;
        }

        public bool ExistsAccount(AdlClient.Models.AnalyticsAccountRef account)
        {
            return this.RestClient.Account.Exists(account.ResourceGroup, account.Name);
        }
    }
}

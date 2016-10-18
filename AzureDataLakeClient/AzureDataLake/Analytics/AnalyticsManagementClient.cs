using System.Collections.Generic;
using System.Linq;
using AzureDataLakeClient.Authentication;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsManagementClient: ClientBase
    {
        private ADL.Analytics.DataLakeAnalyticsAccountManagementClient _adla_mgmt_rest_client;
        private Subscription Sub;

        public AnalyticsManagementClient(Subscription sub, AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Sub = sub;
            this._adla_mgmt_rest_client = new ADL.Analytics.DataLakeAnalyticsAccountManagementClient(this.AuthenticatedSession.Credentials);
            this._adla_mgmt_rest_client.SubscriptionId = sub.ID;
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var initial_page = this._adla_mgmt_rest_client.Account.List();

            foreach (var acc in  RESTUtil.EnumItemsInPages(initial_page,p => this._adla_mgmt_rest_client.Account.ListNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccountsByResourceGroup(string resource_group)
        {
            var initial_page = this._adla_mgmt_rest_client.Account.ListByResourceGroup(resource_group);
            foreach (var acc in RESTUtil.EnumItemsInPages(initial_page, p => this._adla_mgmt_rest_client.Account.ListByResourceGroupNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }
    }
}

namespace Extensions
{
}
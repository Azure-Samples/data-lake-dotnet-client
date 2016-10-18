using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL=Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store;

namespace AzureDataLakeClient.Store
{
    public class StoreManagementClient : ClientBase
    {
        private ADL.Store.DataLakeStoreAccountManagementClient _adls_mgmt_rest_client;
        private Subscription Sub;

        public StoreManagementClient(Subscription sub, AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Sub = sub;
            this._adls_mgmt_rest_client = new ADL.Store.DataLakeStoreAccountManagementClient(this.AuthenticatedSession.Credentials);
            this._adls_mgmt_rest_client.SubscriptionId = sub.ID;
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListAccounts()
        {
            var page = this._adls_mgmt_rest_client.Account.List();
            foreach (var acc in RESTUtil.EnumItemsInPages(page,
                p => this._adls_mgmt_rest_client.Account.ListNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListAccountsByResourceGroup(string resource_group)
        {
            var page = this._adls_mgmt_rest_client.Account.ListByResourceGroup(resource_group);

            foreach (var acc in RESTUtil.EnumItemsInPages(page,
                p => this._adls_mgmt_rest_client.Account.ListByResourceGroupNext(p.NextPageLink)))
            {
                yield return acc;
            }
        }
    }
}
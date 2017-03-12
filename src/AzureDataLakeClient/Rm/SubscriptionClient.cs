using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient
{
    public class SubscriptionClient: ClientBase
    {
        private readonly AzureDataLakeClient.Analytics.Clients.AnalyticsAccountManagmentRestClient _adla_account_mgmt_client;
        private readonly AzureDataLakeClient.Store.StoreAccountManagementClient _adls_account_mgmt_client;
        public readonly AzureDataLakeClient.Rm.Subscription Subscription;

        public SubscriptionClient(AzureDataLakeClient.Rm.Subscription subscription, AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Subscription = subscription;
            this._adla_account_mgmt_client = new AzureDataLakeClient.Analytics.Clients.AnalyticsAccountManagmentRestClient(subscription, authSession.Credentials);
            this._adls_account_mgmt_client = new AzureDataLakeClient.Store.StoreAccountManagementClient(subscription, authSession.Credentials);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAnalyticsAccounts()
        {
            return this._adla_account_mgmt_client.ListAccounts();
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListStoreAccounts()
        {
            return this._adls_account_mgmt_client.ListAccounts();
        }


        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAnalyticsAccountsByResourceGroup(AzureDataLakeClient.Rm.ResourceGroup resource_group)
        {
            return this._adla_account_mgmt_client.ListAccounts(resource_group);
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListStoreAccountsByResourceGroup(AzureDataLakeClient.Rm.ResourceGroup resource_group)
        {
            return this._adls_account_mgmt_client.ListAccounts(resource_group);
        }


        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAnalyticsAccount(AzureDataLakeClient.Analytics.AnalyticsAccount account)
        {
            return this._adla_account_mgmt_client.GetAccount(account.ResourceGroup, account.GetUri());
        }

        public ADL.Store.Models.DataLakeStoreAccount GetStoreAccount(AzureDataLakeClient.Store.StoreAccount account)
        {
            return this._adls_account_mgmt_client.GetAccount(account);
        }

        public bool ExistsAnalyticsAccount(AzureDataLakeClient.Analytics.AnalyticsAccount account)
        {
            return this._adla_account_mgmt_client.ExistsAccount(account.ResourceGroup, account.GetUri());
        }

        public bool ExistsStoreAccount(AzureDataLakeClient.Store.StoreAccount account)
        {
            return this._adls_account_mgmt_client.Exists(account);
        }
    }
}

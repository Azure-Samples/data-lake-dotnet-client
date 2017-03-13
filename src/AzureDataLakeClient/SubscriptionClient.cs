using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient
{
    public class SubscriptionClient: ClientBase
    {
        private readonly AnalyticsAccountManagmentRestWrapper _adlaAccountMgmtClientWrapper;
        private readonly StoreManagementRestWrapper _adls_account_mgmt_client;
        public readonly Subscription Subscription;

        public SubscriptionClient(Subscription subscription, AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Subscription = subscription;
            this._adlaAccountMgmtClientWrapper = new AnalyticsAccountManagmentRestWrapper(subscription, authSession.Credentials);
            this._adls_account_mgmt_client = new StoreManagementRestWrapper(subscription, authSession.Credentials);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAnalyticsAccounts()
        {
            return this._adlaAccountMgmtClientWrapper.ListAccounts();
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListStoreAccounts()
        {
            return this._adls_account_mgmt_client.ListAccounts();
        }


        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAnalyticsAccountsByResourceGroup(ResourceGroup resource_group)
        {
            return this._adlaAccountMgmtClientWrapper.ListAccounts(resource_group);
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListStoreAccountsByResourceGroup(ResourceGroup resource_group)
        {
            return this._adls_account_mgmt_client.ListAccountsByResourceGroup(resource_group);
        }


        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAnalyticsAccount(AnalyticsAccount account)
        {
            return this._adlaAccountMgmtClientWrapper.GetAccount(account.ResourceGroup, account);
        }

        public ADL.Store.Models.DataLakeStoreAccount GetStoreAccount(StoreAccount account)
        {
            return this._adls_account_mgmt_client.GetAccount(account);
        }

        public bool ExistsAnalyticsAccount(AnalyticsAccount account)
        {
            return this._adlaAccountMgmtClientWrapper.ExistsAccount(account.ResourceGroup, account);
        }

        public bool ExistsStoreAccount(StoreAccount account)
        {
            return this._adls_account_mgmt_client.Exists(account);
        }
    }
}

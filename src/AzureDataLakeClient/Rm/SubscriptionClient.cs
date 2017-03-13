using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Rm
{
    public class SubscriptionClient: ClientBase
    {
        private readonly AnalyticsAccountManagmentRestWrapper _adlaAccountMgmtClientWrapper;
        private readonly StoreManagementRestWrapper _adls_account_mgmt_client;
        public readonly AzureDataLakeClient.Rm.Subscription Subscription;

        public SubscriptionClient(AzureDataLakeClient.Rm.Subscription subscription, AuthenticatedSession authSession) :
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


        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAnalyticsAccountsByResourceGroup(AzureDataLakeClient.Rm.ResourceGroup resource_group)
        {
            return this._adlaAccountMgmtClientWrapper.ListAccounts(resource_group);
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListStoreAccountsByResourceGroup(AzureDataLakeClient.Rm.ResourceGroup resource_group)
        {
            return this._adls_account_mgmt_client.ListAccountsByResourceGroup(resource_group);
        }


        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAnalyticsAccount(AzureDataLakeClient.Analytics.AnalyticsAccount account)
        {
            return this._adlaAccountMgmtClientWrapper.GetAccount(account.ResourceGroup, account);
        }

        public ADL.Store.Models.DataLakeStoreAccount GetStoreAccount(AzureDataLakeClient.Store.StoreAccount account)
        {
            return this._adls_account_mgmt_client.GetAccount(account);
        }

        public bool ExistsAnalyticsAccount(AzureDataLakeClient.Analytics.AnalyticsAccount account)
        {
            return this._adlaAccountMgmtClientWrapper.ExistsAccount(account.ResourceGroup, account);
        }

        public bool ExistsStoreAccount(AzureDataLakeClient.Store.StoreAccount account)
        {
            return this._adls_account_mgmt_client.Exists(account);
        }
    }
}

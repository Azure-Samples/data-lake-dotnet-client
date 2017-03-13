using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;

namespace AzureDataLakeClient
{
    public class AnalyticsResourceCommands
    {
        private readonly AnalyticsAccountManagmentRestWrapper _adlaAccountMgmtClientWrapper;
        private readonly StoreManagementRestWrapper _adls_account_mgmt_client;
        public readonly Subscription Subscription;

        public AnalyticsResourceCommands(Subscription subscription, AuthenticatedSession authSession, AnalyticsAccountManagmentRestWrapper w1 )
        {
            this.Subscription = subscription;
            this._adlaAccountMgmtClientWrapper = w1;
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            return this._adlaAccountMgmtClientWrapper.ListAccounts();
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts(ResourceGroup resource_group)
        {
            return this._adlaAccountMgmtClientWrapper.ListAccounts(resource_group);
        }

        public Microsoft.Azure.Management.DataLake.Analytics.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccount account)
        {
            return this._adlaAccountMgmtClientWrapper.GetAccount(account.ResourceGroup, account);
        }

        public bool AccountExists(AnalyticsAccount account)
        {
            return this._adlaAccountMgmtClientWrapper.ExistsAccount(account.ResourceGroup, account);
        }
    }
}
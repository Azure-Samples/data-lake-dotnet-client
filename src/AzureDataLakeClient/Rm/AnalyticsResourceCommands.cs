using System.Collections.Generic;
using AdlClient.Authentication;
using AdlClient.Rest;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient
{
    public class AnalyticsResourceCommands
    {
        private readonly AnalyticsAccountManagmentRestWrapper _adlaAccountMgmtClientWrapper;
        private readonly StoreManagementRestWrapper _adls_account_mgmt_client;
        public readonly Subscription Subscription;
        private AuthenticatedSession auth;

        public AnalyticsResourceCommands(Subscription subscription, AuthenticatedSession authSession, AnalyticsAccountManagmentRestWrapper adla_rest )
        {
            this.Subscription = subscription;
            this._adlaAccountMgmtClientWrapper = adla_rest;
            this.auth = authSession;
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            return this._adlaAccountMgmtClientWrapper.ListAccounts();
        }

        public IEnumerable<MSADLA.Models.DataLakeAnalyticsAccount> ListAccounts(ResourceGroup resource_group)
        {
            return this._adlaAccountMgmtClientWrapper.ListAccounts(resource_group);
        }

        public MSADLA.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccount account)
        {
            return this._adlaAccountMgmtClientWrapper.GetAccount(account.ResourceGroup, account);
        }

        public bool AccountExists(AnalyticsAccount account)
        {
            return this._adlaAccountMgmtClientWrapper.ExistsAccount(account.ResourceGroup, account);
        }
    }
}
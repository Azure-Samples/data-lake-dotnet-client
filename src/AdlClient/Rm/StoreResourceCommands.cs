using System.Collections.Generic;
using AdlClient.Authentication;
using AdlClient.Rest;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient
{
    public class StoreResourceCommands
    {
        private readonly StoreManagementRestWrapper _adls_account_mgmt_client;
        public readonly Subscription Subscription;
        private AuthenticatedSession auth;

        public StoreResourceCommands(Subscription subscription, AuthenticatedSession authSession, StoreManagementRestWrapper adls_rest)
        {
            this.Subscription = subscription;
            this._adls_account_mgmt_client = adls_rest;
            this.auth = authSession;
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccounts()
        {
            return this._adls_account_mgmt_client.ListAccounts();
        }

        public IEnumerable<MSADLS.Models.DataLakeStoreAccount> ListAccounts(ResourceGroup resource_group)
        {
            return this._adls_account_mgmt_client.ListAccountsByResourceGroup(resource_group);
        }

        public MSADLS.Models.DataLakeStoreAccount GetAccount(StoreAccount account)
        {
            return this._adls_account_mgmt_client.GetAccount(account);
        }

        public bool AccountExsists(StoreAccount account)
        {
            return this._adls_account_mgmt_client.Exists(account);
        }

    }
}
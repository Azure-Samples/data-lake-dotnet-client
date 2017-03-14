using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rest;

namespace AzureDataLakeClient
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

        public IEnumerable<Microsoft.Azure.Management.DataLake.Store.Models.DataLakeStoreAccount> ListAccounts()
        {
            return this._adls_account_mgmt_client.ListAccounts();
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Store.Models.DataLakeStoreAccount> ListAccounts(ResourceGroup resource_group)
        {
            return this._adls_account_mgmt_client.ListAccountsByResourceGroup(resource_group);
        }

        public Microsoft.Azure.Management.DataLake.Store.Models.DataLakeStoreAccount GetAccount(StoreAccount account)
        {
            return this._adls_account_mgmt_client.GetAccount(account);
        }

        public bool AccountExsists(StoreAccount account)
        {
            return this._adls_account_mgmt_client.Exists(account);
        }

    }
}
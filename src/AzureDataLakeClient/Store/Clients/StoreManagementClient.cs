using System.Collections.Generic;
using AzureDataLakeClient.Rm;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Store.Clients
{
    public class StoreManagementClient 
    {
        private ADL.Store.DataLakeStoreAccountManagementClient _rest_client;
        private AzureDataLakeClient.Rm.Subscription Sub;
        private StoreManagementRestClient _store_acctmgmt_rest_client;

        public StoreManagementClient(AzureDataLakeClient.Rm.Subscription sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.Sub = sub;
            this._rest_client = new ADL.Store.DataLakeStoreAccountManagementClient(creds);
            this._rest_client.SubscriptionId = sub.ID;
            this._store_acctmgmt_rest_client = new StoreManagementRestClient(_rest_client);
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListAccounts()
        {
            return this._store_acctmgmt_rest_client.ListAccounts();
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListAccountsByResourceGroup(ResourceGroup resource_group)
        {
            return this._store_acctmgmt_rest_client.ListAccountsByResourceGroup(resource_group);
        }

        public ADL.Store.Models.DataLakeStoreAccount GetAccount(StoreAccount account)
        {
            return this._store_acctmgmt_rest_client.GetAccount(account);
        }

        public void Delete(StoreAccount account)
        {
            this._store_acctmgmt_rest_client.Delete(account);
        }

        public bool Exists(StoreAccount account)
        {
            return this._store_acctmgmt_rest_client.Exists(account);
        }

    }
}
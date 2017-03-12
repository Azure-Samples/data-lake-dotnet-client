using System.Collections.Generic;
using AzureDataLakeClient.Rm;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Store
{
    public class StoreAccountManagementClient 
    {
        private StoreManagementRestClient _store_acctmgmt_client;
        private ADL.Store.DataLakeStoreAccountManagementClient _store_acctmgmt_rest_client;
        private AzureDataLakeClient.Rm.Subscription Sub;

        public StoreAccountManagementClient(AzureDataLakeClient.Rm.Subscription sub, Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.Sub = sub;
            this._store_acctmgmt_rest_client = new ADL.Store.DataLakeStoreAccountManagementClient(creds);
            this._store_acctmgmt_rest_client.SubscriptionId = sub.ID;
            this._store_acctmgmt_client = new StoreManagementRestClient(_store_acctmgmt_rest_client);
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListAccounts()
        {
            return this._store_acctmgmt_client.ListAccounts();
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListAccounts(ResourceGroup resource_group)
        {
            return this._store_acctmgmt_client.ListAccountsByResourceGroup(resource_group);
        }

        public ADL.Store.Models.DataLakeStoreAccount GetAccount(StoreAccount account)
        {
            return this._store_acctmgmt_client.GetAccount(account);
        }

        public void Delete(StoreAccount account)
        {
            this._store_acctmgmt_client.Delete(account);
        }

        public bool Exists(StoreAccount account)
        {
            return this._store_acctmgmt_client.Exists(account);
        }

    }
}
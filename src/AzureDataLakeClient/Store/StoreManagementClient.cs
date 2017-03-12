using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Rm;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Store
{
    public class StoreManagementClient : ClientBase
    {
        private ADL.Store.DataLakeStoreAccountManagementClient _rest_client;
        private AzureDataLakeClient.Rm.Subscription Sub;
        private StoreManagementRestClient _storeManagement;

        public StoreManagementClient(AzureDataLakeClient.Rm.Subscription sub, AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Sub = sub;
            this._rest_client = new ADL.Store.DataLakeStoreAccountManagementClient(this.AuthenticatedSession.Credentials);
            this._rest_client.SubscriptionId = sub.ID;
            this._storeManagement = new StoreManagementRestClient(_rest_client);
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListAccounts()
        {
            return this._storeManagement.ListAccounts();
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListAccountsByResourceGroup(ResourceGroup resource_group)
        {
            return this._storeManagement.ListAccountsByResourceGroup(resource_group);
        }

        public ADL.Store.Models.DataLakeStoreAccount GetAccount(StoreAccount account)
        {
            return this._storeManagement.GetAccount(account);
        }

        public void Update(StoreAccount account, ADL.Store.Models.DataLakeStoreAccountUpdateParameters parameters)
        {
            this._storeManagement.Update(account, parameters);
        }

        public void Delete(StoreAccount account)
        {
            this._storeManagement.Delete(account);
        }
    }
}
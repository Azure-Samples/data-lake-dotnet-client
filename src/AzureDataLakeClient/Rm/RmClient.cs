using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient
{
    public class RmClient: ClientBase
    {
        private AzureDataLakeClient.Analytics.Clients.AnalyticsAccountManagmentClient _adla_account_mgmt_client;
        private AzureDataLakeClient.Store.StoreManagementClient _adls_account_mgmt_client;
        private AzureDataLakeClient.Rm.Subscription Sub;

        public RmClient(AzureDataLakeClient.Rm.Subscription sub, AuthenticatedSession authSession) :
            base(authSession)
        {

            this.Sub = sub;
            this._adla_account_mgmt_client = new AzureDataLakeClient.Analytics.Clients.AnalyticsAccountManagmentClient(sub, authSession.Credentials);
            this._adls_account_mgmt_client = new AzureDataLakeClient.Store.StoreManagementClient(sub,
                authSession.Credentials);
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
            return this._adla_account_mgmt_client.ListAccountsByResourceGroup(resource_group);
        }

        public IEnumerable<ADL.Store.Models.DataLakeStoreAccount> ListStoreAccountsByResourceGroup(AzureDataLakeClient.Rm.ResourceGroup resource_group)
        {
            return this._adls_account_mgmt_client.ListAccountsByResourceGroup(resource_group);
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

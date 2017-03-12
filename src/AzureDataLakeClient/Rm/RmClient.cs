using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient
{
    public class RmClient: ClientBase
    {
        private AzureDataLakeClient.Analytics.Clients.AnalyticsAccountManagmentClient _rest_client;
        private AzureDataLakeClient.Rm.Subscription Sub;

        public RmClient(AzureDataLakeClient.Rm.Subscription sub, AuthenticatedSession authSession) :
            base(authSession)
        {

            this.Sub = sub;
            this._rest_client = new AzureDataLakeClient.Analytics.Clients.AnalyticsAccountManagmentClient(sub, authSession.Credentials);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            return this._rest_client.ListAccounts();
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccountsByResourceGroup(AzureDataLakeClient.Rm.ResourceGroup resource_group)
        {
            return this._rest_client.ListAccountsByResourceGroup(resource_group);
        }

        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAccount(AzureDataLakeClient.Analytics.AnalyticsAccount account)
        {
            return this._rest_client.GetAccount(account.ResourceGroup, account.GetUri());
        }

        public bool ExistsAccount(AzureDataLakeClient.Analytics.AnalyticsAccount account)
        {
            return this._rest_client.ExistsAccount(account.ResourceGroup, account.GetUri());
        }

    }
}

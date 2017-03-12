using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsRmClient: ClientBase
    {
        private AnalyticsRmRestClient _rest_client;
        private AzureDataLakeClient.Rm.Subscription Sub;

        public AnalyticsRmClient(AzureDataLakeClient.Rm.Subscription sub, AuthenticatedSession authSession) :
            base(authSession)
        {

            this.Sub = sub;
            this._rest_client = new AnalyticsRmRestClient(sub, authSession.Credentials);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            return this._rest_client.ListAccounts();
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccountsByResourceGroup(AzureDataLakeClient.Rm.ResourceGroup resource_group)
        {
            return this._rest_client.ListAccountsByResourceGroup(resource_group);
        }

        public ADL.Analytics.Models.DataLakeAnalyticsAccount GetAccount(AnalyticsAccount account)
        {
            return this._rest_client.GetAccount(account.ResourceGroup, account.GetUri());
        }

        public bool ExistsAccount(AnalyticsAccount account)
        {
            return this._rest_client.ExistsAccount(account.ResourceGroup, account.GetUri());
        }

    }
}

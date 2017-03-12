using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{

    // --------------------------


    public class AnalyticsAccountClient : AccountClientBase
    {
        // The maximum page size for ADLA list is 300

        public static int ADLJobPageSize = 300;

        private AnalyticsJobsRestClient _adla_job_rest_client;
        private AnalyticsCatalogRestClient _adla_catalog_rest_client;
        private AnalyticsRmRestClient _rest_client;

        AnalyticsAccountUri analyticsuri;


        public readonly JobCommands Jobs;
        public readonly CatalogCommands Catalog;

        public AnalyticsAccountClient(AnalyticsAccount account, AuthenticatedSession authSession) :
            base(account.Name, authSession)
        {
            this._adla_job_rest_client = new AnalyticsJobsRestClient(this.AuthenticatedSession.Credentials);
            this._adla_catalog_rest_client = new AnalyticsCatalogRestClient(this.AuthenticatedSession.Credentials);
            this._rest_client = new AnalyticsRmRestClient(account.Subscription, authSession.Credentials);
            this.analyticsuri = account.GetUri();

            this.Jobs = new JobCommands(account, this._adla_job_rest_client, authSession);
            this.Catalog = new CatalogCommands(account, this._adla_catalog_rest_client, authSession);
        }



        public void UpdateAccount(AnalyticsAccount account, ADL.Analytics.Models.DataLakeAnalyticsAccountUpdateParameters parameters)
        {
            this._rest_client.UpdateAccount(account.ResourceGroup, account.GetUri(), parameters);
        }

        public void LinkBlobStorageAccount(AnalyticsAccount account, string storage_account, ADL.Analytics.Models.AddStorageAccountParameters parameters)
        {
            this._rest_client.AddStorageAccount(account.ResourceGroup, account.GetUri(), storage_account, parameters);
        }

        public void LinkDataLakeStoreAccount(AnalyticsAccount account, string storage_account, ADL.Analytics.Models.AddDataLakeStoreParameters parameters)
        {
            this._rest_client.AddDataLakeStoreAccount(account.ResourceGroup, account.GetUri(), storage_account, parameters);
        }

        public IEnumerable<ADL.Analytics.Models.DataLakeStoreAccountInfo> ListLinkedDataLakeStoreAccounts(AnalyticsAccount account)
        {
            return this._rest_client.ListStoreAccounts(account.ResourceGroup, account.GetUri());
        }

        public IEnumerable<ADL.Analytics.Models.StorageAccountInfo> ListLinkedBlobStorageAccounts(AnalyticsAccount account)
        {
            return this._rest_client.ListStorageAccounts(account.ResourceGroup, account.GetUri());
        }

        public IEnumerable<ADL.Analytics.Models.StorageContainer> ListLinkedBlobStorageContainers(AnalyticsAccount account, string storage_account)
        {
            return this._rest_client.ListStorageContainers(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public void UnlinkBlobStorageAccount(AnalyticsAccount account, string storage_account)
        {
            this._rest_client.DeleteStorageAccount(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public void UnlinkDataLakeStoreAccount(AnalyticsAccount account, string storage_account)
        {
            this._rest_client.DeleteDataLakeStoreAccount(account.ResourceGroup, account.GetUri(), storage_account);
        }

        public IEnumerable<ADL.Analytics.Models.SasTokenInfo> ListBlobStorageSasTokens(AnalyticsAccount account, string storage_account, string container)
        {
            return this._rest_client.ListSasTokens(account.ResourceGroup, account.GetUri(), storage_account, container);
        }

    }
}
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

        public AnalyticsAccountClient(AnalyticsAccount account, AuthenticatedSession authSession) :
            base(account.Name, authSession)
        {
            this._adla_job_rest_client = new AnalyticsJobsRestClient(this.AuthenticatedSession.Credentials);
            this._adla_catalog_rest_client = new AnalyticsCatalogRestClient(this.AuthenticatedSession.Credentials);
            this._rest_client = new AnalyticsRmRestClient(account.Subscription, authSession.Credentials);
            this.analyticsuri = account.GetUri();

            this.Jobs = new JobCommands(account, this._adla_job_rest_client, authSession);
        }


        public ADL.Analytics.Models.USqlDatabase GetDatabase(string name)
        {
            var db = this._adla_catalog_rest_client.GetDatabase(this.analyticsuri, name);
            return db;
        }

        public IEnumerable<ADL.Analytics.Models.USqlDatabase> ListDatabases()
        {
            return this._adla_catalog_rest_client.ListDatabases(this.analyticsuri);
        }

        public IEnumerable<ADL.Analytics.Models.USqlAssemblyClr> ListAssemblies(string dbname)
        {
            return this._adla_catalog_rest_client.ListAssemblies(this.analyticsuri, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(string dbname)
        {
            return this._adla_catalog_rest_client.ListExternalDatasources(this.analyticsuri, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlProcedure> ListProcedures(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListProcedures(this.analyticsuri, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlSchema> ListSchemas(string dbname)
        {
            return this._adla_catalog_rest_client.ListSchemas(this.analyticsuri, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlView> ListViews(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListViews(this.analyticsuri, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTable> ListTables(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListTables(this.analyticsuri, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlType> ListTypes(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListTypes(this.analyticsuri, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableType> ListTableTypes(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListTableTypes(this.analyticsuri, dbname, schema);
        }

        public void CreateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this._adla_catalog_rest_client.CreateCredential(this.analyticsuri, dbname, credname, create_parameters);
        }

        public void DeleteCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this._adla_catalog_rest_client.DeleteCredential(this.analyticsuri, dbname, credname, delete_parameters);
        }

        public void UpdateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this._adla_catalog_rest_client.UpdateCredential(this.analyticsuri, dbname, credname, update_parameters);
        }

        public ADL.Analytics.Models.USqlCredential GetCredential(string dbname, string credname)
        {
            return this._adla_catalog_rest_client.GetCredential(this.analyticsuri, dbname, credname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlCredential> ListCredential(string dbname)
        {
            return this._adla_catalog_rest_client.ListCredential(this.analyticsuri, dbname);
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
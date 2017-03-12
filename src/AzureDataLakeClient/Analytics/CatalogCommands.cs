using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class CatalogCommands
    {
        private AnalyticsCatalogRestClient _adla_catalog_rest_client;
        private AnalyticsAccount account;
        AuthenticatedSession authSession;

        public CatalogCommands(AnalyticsAccount a, AnalyticsCatalogRestClient c, AuthenticatedSession authSession)
        {
            this.account = a;
            this._adla_catalog_rest_client = c;
            this.authSession = authSession;
        }

        public ADL.Analytics.Models.USqlDatabase GetDatabase(string name)
        {
            var db = this._adla_catalog_rest_client.GetDatabase(this.account.GetUri(), name);
            return db;
        }

        public IEnumerable<ADL.Analytics.Models.USqlDatabase> ListDatabases()
        {
            return this._adla_catalog_rest_client.ListDatabases(this.account.GetUri());
        }

        public IEnumerable<ADL.Analytics.Models.USqlAssemblyClr> ListAssemblies(string dbname)
        {
            return this._adla_catalog_rest_client.ListAssemblies(this.account.GetUri(), dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(string dbname)
        {
            return this._adla_catalog_rest_client.ListExternalDatasources(this.account.GetUri(), dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlProcedure> ListProcedures(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListProcedures(this.account.GetUri(), dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlSchema> ListSchemas(string dbname)
        {
            return this._adla_catalog_rest_client.ListSchemas(this.account.GetUri(), dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlView> ListViews(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListViews(this.account.GetUri(), dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTable> ListTables(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListTables(this.account.GetUri(), dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlType> ListTypes(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListTypes(this.account.GetUri(), dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableType> ListTableTypes(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListTableTypes(this.account.GetUri(), dbname, schema);
        }

        public void CreateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this._adla_catalog_rest_client.CreateCredential(this.account.GetUri(), dbname, credname, create_parameters);
        }

        public void DeleteCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this._adla_catalog_rest_client.DeleteCredential(this.account.GetUri(), dbname, credname, delete_parameters);
        }

        public void UpdateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this._adla_catalog_rest_client.UpdateCredential(this.account.GetUri(), dbname, credname, update_parameters);
        }

        public ADL.Analytics.Models.USqlCredential GetCredential(string dbname, string credname)
        {
            return this._adla_catalog_rest_client.GetCredential(this.account.GetUri(), dbname, credname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlCredential> ListCredential(string dbname)
        {
            return this._adla_catalog_rest_client.ListCredential(this.account.GetUri(), dbname);
        }

    }
}
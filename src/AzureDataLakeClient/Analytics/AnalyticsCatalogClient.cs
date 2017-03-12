using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsCatalogClient : AccountClientBase
    {
        private AnalyticsCatalogRestClient _adla_catalog_rest_client;

        public AnalyticsCatalogClient(string account, AuthenticatedSession authSession) :
            base(account, authSession)
        {
            this._adla_catalog_rest_client = new AnalyticsCatalogRestClient(this.AuthenticatedSession.Credentials);
        }

        public ADL.Analytics.Models.USqlDatabase GetDatabase(string name)
        {
            var db = this._adla_catalog_rest_client.GetDatabase(this.Account, name);
            return db;
        }

        public IEnumerable<ADL.Analytics.Models.USqlDatabase> ListDatabases()
        {
            return this._adla_catalog_rest_client.ListDatabases(this.Account);
        }

        public IEnumerable<ADL.Analytics.Models.USqlAssemblyClr> ListAssemblies(string dbname)
        {
            return this._adla_catalog_rest_client.ListAssemblies(this.Account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(string dbname)
        {
            return this._adla_catalog_rest_client.ListExternalDatasources(this.Account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlProcedure> ListProcedures(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListProcedures(this.Account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlSchema> ListSchemas(string dbname)
        {
            return this._adla_catalog_rest_client.ListSchemas(this.Account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlView> ListViews(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListViews(this.Account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTable> ListTables(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListTables(this.Account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlType> ListTypes(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListTypes(this.Account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableType> ListTableTypes(string dbname, string schema)
        {
            return this._adla_catalog_rest_client.ListTableTypes(this.Account, dbname, schema);
        }

        public void CreateCredential(string dbname, string credname, DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this._adla_catalog_rest_client.CreateCredential(this.Account, dbname, credname, create_parameters);
        }

        public void DeleteCredential(string dbname, string credname, DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this._adla_catalog_rest_client.DeleteCredential(this.Account, dbname, credname, delete_parameters);
        }

        public void UpdateCredential(string dbname, string credname, DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this._adla_catalog_rest_client.UpdateCredential(this.Account, dbname, credname, update_parameters);
        }
        
        public ADL.Analytics.Models.USqlCredential GetCredential(string dbname, string credname)
        {
            return this._adla_catalog_rest_client.GetCredential(this.Account, dbname, credname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlCredential> ListCredential(string dbname)
        {
            return this._adla_catalog_rest_client.ListCredential(this.Account, dbname);
        }
    }
}
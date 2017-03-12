using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsCatalogRestClient
    {
        private Microsoft.Azure.Management.DataLake.Analytics.DataLakeAnalyticsCatalogManagementClient _client;
        private Microsoft.Rest.ServiceClientCredentials _creds;

        public AnalyticsCatalogRestClient(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._creds = creds;
            this._client = new Microsoft.Azure.Management.DataLake.Analytics.DataLakeAnalyticsCatalogManagementClient(creds);
        }

        public Microsoft.Azure.Management.DataLake.Analytics.Models.USqlDatabase GetDatabase(string account, string name)
        {
            var db = this._client.Catalog.GetDatabase(account, name);
            return db;
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlDatabase> ListDatabases(string account)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlDatabase>();

            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListDatabases(account, oDataQuery, @select, count);
            foreach (var db in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlDatabase>(page, p => this._client.Catalog.ListDatabasesNext(p.NextPageLink)))
            {
                yield return db;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlAssemblyClr> ListAssemblies(string account, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlAssembly>();

            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListAssemblies(account, dbname, oDataQuery, @select, count);
            foreach (var asm in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlAssemblyClr>(page, p => this._client.Catalog.ListAssembliesNext(p.NextPageLink)))
            {
                yield return asm;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(string account, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlExternalDataSource>();

            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListExternalDataSources(account, dbname, oDataQuery, @select, count);
            foreach (var ds in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlExternalDataSource>(page, p => this._client.Catalog.ListExternalDataSourcesNext(p.NextPageLink)))
            {
                yield return ds;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlProcedure> ListProcedures(string account, string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlProcedure>();

            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListProcedures(account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlProcedure>(page, p => this._client.Catalog.ListProceduresNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlSchema> ListSchemas(string account, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlSchema>();
            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListSchemas(account, dbname, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlSchema>(page, p => this._client.Catalog.ListSchemasNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlView> ListViews(string account,string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlView>();
            string @select = null;
            bool? count = null;


            var page = this._client.Catalog.ListViews(account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlView>(page, p => this._client.Catalog.ListViewsNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlTable> ListTables(string account, string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlTable>();
            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListTables(account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlTable>(page, p => this._client.Catalog.ListTablesNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlType> ListTypes(string account, string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlType>();
            string @select = null;
            bool? count = null;


            var page = this._client.Catalog.ListTypes(account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlType>(page, p => this._client.Catalog.ListTypesNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlTableType> ListTableTypes(string account, string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlTableType>();
            string @select = null;
            bool? count = null;


            var page = this._client.Catalog.ListTableTypes(account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlTableType>(page, p => this._client.Catalog.ListTableTypesNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public void CreateCredential(string account, string dbname, string credname, DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this._client.Catalog.CreateCredential(account, dbname, credname, create_parameters);
        }

        public void DeleteCredential(string account, string dbname, string credname, DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this._client.Catalog.DeleteCredential(account, dbname, credname);
        }

        public void UpdateCredential(string account, string dbname, string credname, DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this._client.Catalog.UpdateCredential(account, dbname, credname, update_parameters);
        }

        public Microsoft.Azure.Management.DataLake.Analytics.Models.USqlCredential GetCredential(string account, string dbname, string credname)
        {
            return this._client.Catalog.GetCredential(account, dbname, credname);
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlCredential> ListCredential(string account, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlCredential>();
            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListCredentials(account, dbname, oDataQuery, @select, count);
            foreach (var cred in RESTUtil.EnumItemsInPages<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlCredential>(page, p => this._client.Catalog.ListCredentialsNext(p.NextPageLink)))
            {
                yield return cred;
            }
        }

    }
}
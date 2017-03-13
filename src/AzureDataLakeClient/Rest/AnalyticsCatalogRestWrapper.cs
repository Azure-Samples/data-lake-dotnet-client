using System.Collections.Generic;
using AzureDataLakeClient.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using MSADLA=Microsoft.Azure.Management.DataLake.Analytics;

namespace AzureDataLakeClient.Rest
{
    public class AnalyticsCatalogRestWrapper
    {
        private MSADLA.DataLakeAnalyticsCatalogManagementClient _client;
        private Microsoft.Rest.ServiceClientCredentials _creds;

        public AnalyticsCatalogRestWrapper(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._creds = creds;
            this._client = new MSADLA.DataLakeAnalyticsCatalogManagementClient(creds);
        }

        public MSADLA.Models.USqlDatabase GetDatabase(AnalyticsAccount account, string name)
        {
            var db = this._client.Catalog.GetDatabase(account.Name, name);
            return db;
        }

        public IEnumerable<MSADLA.Models.USqlDatabase> ListDatabases(AnalyticsAccount account)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.USqlDatabase>();

            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListDatabases(account.Name, oDataQuery, @select, count);
            foreach (var db in RestUtil.EnumItemsInPages<MSADLA.Models.USqlDatabase>(page, p => this._client.Catalog.ListDatabasesNext(p.NextPageLink)))
            {
                yield return db;
            }
        }

        public IEnumerable<MSADLA.Models.USqlAssemblyClr> ListAssemblies(AnalyticsAccount account, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlAssembly>();

            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListAssemblies(account.Name, dbname, oDataQuery, @select, count);
            foreach (var asm in RestUtil.EnumItemsInPages<MSADLA.Models.USqlAssemblyClr>(page, p => this._client.Catalog.ListAssembliesNext(p.NextPageLink)))
            {
                yield return asm;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(AnalyticsAccount account, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.USqlExternalDataSource>();

            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListExternalDataSources(account.Name, dbname, oDataQuery, @select, count);
            foreach (var ds in RestUtil.EnumItemsInPages<MSADLA.Models.USqlExternalDataSource>(page, p => this._client.Catalog.ListExternalDataSourcesNext(p.NextPageLink)))
            {
                yield return ds;
            }
        }

        public IEnumerable<MSADLA.Models.USqlProcedure> ListProcedures(AnalyticsAccount account, string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.USqlProcedure>();

            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListProcedures(account.Name, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RestUtil.EnumItemsInPages<MSADLA.Models.USqlProcedure>(page, p => this._client.Catalog.ListProceduresNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<MSADLA.Models.USqlSchema> ListSchemas(AnalyticsAccount account, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.USqlSchema>();
            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListSchemas(account.Name, dbname, oDataQuery, @select, count);
            foreach (var proc in RestUtil.EnumItemsInPages<MSADLA.Models.USqlSchema>(page, p => this._client.Catalog.ListSchemasNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlView> ListViews(AnalyticsAccount account,string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<Microsoft.Azure.Management.DataLake.Analytics.Models.USqlView>();
            string @select = null;
            bool? count = null;


            var page = this._client.Catalog.ListViews(account.Name, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RestUtil.EnumItemsInPages<MSADLA.Models.USqlView>(page, p => this._client.Catalog.ListViewsNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<MSADLA.Models.USqlTable> ListTables(AnalyticsAccount account, string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.USqlTable>();
            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListTables(account.Name, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RestUtil.EnumItemsInPages<MSADLA.Models.USqlTable>(page, p => this._client.Catalog.ListTablesNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<MSADLA.Models.USqlType> ListTypes(AnalyticsAccount account, string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.USqlType>();
            string @select = null;
            bool? count = null;


            var page = this._client.Catalog.ListTypes(account.Name, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RestUtil.EnumItemsInPages<MSADLA.Models.USqlType>(page, p => this._client.Catalog.ListTypesNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<MSADLA.Models.USqlTableType> ListTableTypes(AnalyticsAccount account, string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.USqlTableType>();
            string @select = null;
            bool? count = null;


            var page = this._client.Catalog.ListTableTypes(account.Name, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RestUtil.EnumItemsInPages<MSADLA.Models.USqlTableType>(page, p => this._client.Catalog.ListTableTypesNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public void CreateCredential(AnalyticsAccount account, string dbname, string credname, DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this._client.Catalog.CreateCredential(account.Name, dbname, credname, create_parameters);
        }

        public void DeleteCredential(AnalyticsAccount account, string dbname, string credname, DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this._client.Catalog.DeleteCredential(account.Name, dbname, credname);
        }

        public void UpdateCredential(AnalyticsAccount account, string dbname, string credname, DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this._client.Catalog.UpdateCredential(account.Name, dbname, credname, update_parameters);
        }

        public MSADLA.Models.USqlCredential GetCredential(AnalyticsAccount account, string dbname, string credname)
        {
            return this._client.Catalog.GetCredential(account.Name, dbname, credname);
        }

        public IEnumerable<MSADLA.Models.USqlCredential> ListCredential(AnalyticsAccount account, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<MSADLA.Models.USqlCredential>();
            string @select = null;
            bool? count = null;

            var page = this._client.Catalog.ListCredentials(account.Name, dbname, oDataQuery, @select, count);
            foreach (var cred in RestUtil.EnumItemsInPages<MSADLA.Models.USqlCredential>(page, p => this._client.Catalog.ListCredentialsNext(p.NextPageLink)))
            {
                yield return cred;
            }
        }

    }
}
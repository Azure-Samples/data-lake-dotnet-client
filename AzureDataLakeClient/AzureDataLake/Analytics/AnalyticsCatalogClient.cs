using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Analytics
{
    public class AnalyticsCatalogClient : AccountClientBase
    {
        private ADL.Analytics.IDataLakeAnalyticsCatalogManagementClient _adla_catalog_rest_client;
        private ICatalogOperations _catalog_ops;

        public AnalyticsCatalogClient(string account, AuthenticatedSession authSession) :
            base(account, authSession)
        {
            this._adla_catalog_rest_client = new ADL.Analytics.DataLakeAnalyticsCatalogManagementClient(this.AuthenticatedSession.Credentials);
            this._catalog_ops = this._adla_catalog_rest_client.Catalog;
        }

        public ADL.Analytics.Models.USqlDatabase GetDatabase(GetJobsOptions options, string name)
        {
            var db = this._adla_catalog_rest_client.Catalog.GetDatabase(this.Account, name);
            return db;
        }

        public IEnumerable<ADL.Analytics.Models.USqlDatabase> ListDatabases()
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlDatabase>();

            string @select = null;
            bool? count = null;
            
            var page = _catalog_ops.ListDatabases(this.Account, oDataQuery, @select, count);
            foreach (var db in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.USqlDatabase> (page, p => _catalog_ops.ListDatabasesNext(p.NextPageLink)))
            {
                yield return db;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlAssemblyClr> ListAssemblies(string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlAssembly>();

            string @select = null;
            bool? count = null;

            var page = _catalog_ops.ListAssemblies(this.Account, dbname, oDataQuery, @select, count);
            foreach (var asm in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.USqlAssemblyClr>(page, p => _catalog_ops.ListAssembliesNext(p.NextPageLink)))
            {
                yield return asm;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlExternalDataSource>();

            string @select = null;
            bool? count = null;

            var page = _catalog_ops.ListExternalDataSources(this.Account, dbname, oDataQuery, @select, count);
            foreach (var ds in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.USqlExternalDataSource>(page, p => _catalog_ops.ListExternalDataSourcesNext(p.NextPageLink)))
            {
                yield return ds;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlProcedure> ListProcedures(string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlProcedure>();

            string @select = null;
            bool? count = null;

            var page = _catalog_ops.ListProcedures(this.Account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.USqlProcedure>(page, p => _catalog_ops.ListProceduresNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlSchema> ListSchemas(string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlSchema>();
            string @select = null;
            bool? count = null;

            var page = _catalog_ops.ListSchemas(this.Account, dbname, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.USqlSchema>(page, p => _catalog_ops.ListSchemasNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlView> ListViews(string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlView>();
            string @select = null;
            bool? count = null;


            var page = _catalog_ops.ListViews(this.Account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.USqlView> (page, p => _catalog_ops.ListViewsNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlTable> ListTables(string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlTable>();
            string @select = null;
            bool? count = null;


            var page = _catalog_ops.ListTables(this.Account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.USqlTable>(page, p => _catalog_ops.ListTablesNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlType> ListTypes(string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlType>();
            string @select = null;
            bool? count = null;


            var page = _catalog_ops.ListTypes(this.Account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.USqlType>(page, p => _catalog_ops.ListTypesNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableType> ListTableTypes(string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlTableType>();
            string @select = null;
            bool? count = null;


            var page = this._catalog_ops.ListTableTypes(this.Account, dbname, schema, oDataQuery, @select, count);
            foreach (var proc in RESTUtil.EnumItemsInPages<ADL.Analytics.Models.USqlTableType>(page, p => this._catalog_ops.ListTableTypesNext(p.NextPageLink)))
            {
                yield return proc;
            }
        }


        public ADL.Analytics.Models.USqlSecret GetSecret(string dbname, string secret_name)
        {
            return this._catalog_ops.GetSecret(this.Account, dbname, secret_name);
        }

        public void CreateSecret(string dbname, string secret_name, DataLakeAnalyticsCatalogSecretCreateOrUpdateParameters secret_parameters)
        {
            this._catalog_ops.CreateSecret(this.Account, dbname, secret_name, secret_parameters);
        }

        public void DeleteSecret(string dbname, string secret_name)
        {
            this._catalog_ops.DeleteSecret(this.Account, dbname, secret_name);
        }

        public void DeleteAllSecrets(string dbname)
        {
            this._catalog_ops.DeleteAllSecrets(this.Account, dbname);
        }
    }
}
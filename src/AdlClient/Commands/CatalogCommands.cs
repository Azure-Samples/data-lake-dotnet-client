using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;
using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL = Microsoft.Azure.Management.DataLake;
using MSODATA = Microsoft.Rest.Azure.OData;

namespace AdlClient.Commands
{
    public class CatalogCommands
    {
        internal readonly AdlClient.Models.AnalyticsAccountRef Account;
        internal readonly AnalyticsRestClients RestClients;

        internal CatalogCommands(AdlClient.Models.AnalyticsAccountRef accout, AnalyticsRestClients restclients)
        {
            this.Account = accout;
            this.RestClients = restclients;
        }

        public ADL.Analytics.Models.USqlDatabase GetDatabase(string dbname)
        {
            var db = this.RestClients.CatalogClient.Catalog.GetDatabase(this.Account.Name, dbname);
            return db;
        }

        public IEnumerable<ADL.Analytics.Models.USqlDatabase> ListDatabases()
        {
            var oDataQuery = new MSODATA.ODataQuery<ADL.Analytics.Models.USqlDatabase>();

            string select_cols = null;
            bool? count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlDatabase>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListDatabases(this.Account.Name, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListDatabasesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlAssemblyClr> ListAssemblies(string dbname)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlAssembly>();

            string select_cols = null;
            bool? count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlAssemblyClr>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListAssemblies(this.Account.Name, dbname, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListAssembliesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(string dbname)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlExternalDataSource>();

            string select_cols = null;
            bool? count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlExternalDataSource>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListExternalDataSources(this.Account.Name, dbname, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListExternalDataSourcesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlProcedure> ListProcedures(string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlProcedure>();

            string select_cols = null;
            bool? count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlProcedure>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListProcedures(this.Account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListProceduresNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlSchema> ListSchemas(string dbname)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlSchema>();
            string select_cols = null;
            bool? count = null;



            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlSchema>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListSchemas(this.Account.Name, dbname, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListSchemasNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlView> ListViews(string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlView>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlView>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListViews(this.Account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListViewsNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlTable> ListTables(string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlTable>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlTable>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListTables(this.Account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListTablesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlType> ListTypes(string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlType>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlType>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListTypes(this.Account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListTypesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableType> ListTableTypes(string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlTableType>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlTableType>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListTableTypes(this.Account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListTableTypesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlTablePartition> ListTablePartitions(string dbname, string schema, string tablename)
        {
            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlTablePartition>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListTablePartitions(this.Account.Name, dbname, schema, tablename);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListTablePartitionsNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableStatistics> ListTableStatistics(string dbname, string schema, string tablename)
        {
            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlTableStatistics>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListTableStatistics(this.Account.Name, dbname, schema, tablename);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListTableStatisticsNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public void CreateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this.RestClients.CatalogClient.Catalog.CreateCredential(this.Account.Name, dbname, credname, create_parameters);
        }

        public void DeleteCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this.RestClients.CatalogClient.Catalog.DeleteCredential(this.Account.Name, dbname, credname);
        }

        public void UpdateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this.RestClients.CatalogClient.Catalog.UpdateCredential(this.Account.Name, dbname, credname, update_parameters);
        }

        public ADL.Analytics.Models.USqlCredential GetCredential(string dbname, string credname)
        {
            return this.RestClients.CatalogClient.Catalog.GetCredential(this.Account.Name, dbname, credname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlCredential> ListCredential(string dbname)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlCredential>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new Rest.PagedIterator<MSADLA.Models.USqlCredential>();
            pageiter.GetFirstPage = () => this.RestClients.CatalogClient.Catalog.ListCredentials(this.Account.Name, dbname, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClients.CatalogClient.Catalog.ListCredentialsNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }
    }
}
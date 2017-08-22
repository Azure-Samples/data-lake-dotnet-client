using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using MSADLA=Microsoft.Azure.Management.DataLake.Analytics;
using MSODATA = Microsoft.Rest.Azure.OData;

namespace AdlClient.Rest
{
    public class AnalyticsCatalogRestWrapper
    {
        public readonly MSADLA.DataLakeAnalyticsCatalogManagementClient RestClient;

        public AnalyticsCatalogRestWrapper(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.RestClient = new MSADLA.DataLakeAnalyticsCatalogManagementClient(creds);
        }

        public MSADLA.Models.USqlDatabase GetDatabase(AdlClient.Models.AnalyticsAccountRef account, string name)
        {
            var db = this.RestClient.Catalog.GetDatabase(account.Name, name);
            return db;
        }

        public IEnumerable<MSADLA.Models.USqlDatabase> ListDatabases(AdlClient.Models.AnalyticsAccountRef account)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlDatabase>();

            string select_cols = null;
            bool? count = null;

            var pageiter = new PagedIterator<MSADLA.Models.USqlDatabase>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListDatabases(account.Name, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListDatabasesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<MSADLA.Models.USqlAssemblyClr> ListAssemblies(AdlClient.Models.AnalyticsAccountRef account, string dbname)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlAssembly>();

            string select_cols = null;
            bool? count = null;

            var pageiter = new PagedIterator<MSADLA.Models.USqlAssemblyClr>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListAssemblies(account.Name, dbname, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListAssembliesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<MSADLA.Models.USqlExternalDataSource> ListExternalDatasources(AdlClient.Models.AnalyticsAccountRef account, string dbname)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlExternalDataSource>();

            string select_cols = null;
            bool? count = null;

            var pageiter = new PagedIterator<MSADLA.Models.USqlExternalDataSource>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListExternalDataSources(account.Name, dbname, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListExternalDataSourcesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;

        }

        public IEnumerable<MSADLA.Models.USqlProcedure> ListProcedures(AdlClient.Models.AnalyticsAccountRef account, string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlProcedure>();

            string select_cols = null;
            bool? count = null;
           
            var pageiter = new PagedIterator<MSADLA.Models.USqlProcedure>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListProcedures(account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListProceduresNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;

        }

        public IEnumerable<MSADLA.Models.USqlSchema> ListSchemas(AdlClient.Models.AnalyticsAccountRef account, string dbname)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlSchema>();
            string select_cols = null;
            bool? count = null;

            
         
            var pageiter = new PagedIterator<MSADLA.Models.USqlSchema>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListSchemas(account.Name, dbname, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListSchemasNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;

        }

        public IEnumerable<MSADLA.Models.USqlView> ListViews(AdlClient.Models.AnalyticsAccountRef account,string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlView>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new PagedIterator<MSADLA.Models.USqlView>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListViews(account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListViewsNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;

        }

        public IEnumerable<MSADLA.Models.USqlTable> ListTables(AdlClient.Models.AnalyticsAccountRef account, string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlTable>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new PagedIterator<MSADLA.Models.USqlTable>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListTables(account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListTablesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<MSADLA.Models.USqlType> ListTypes(AdlClient.Models.AnalyticsAccountRef account, string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlType>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new PagedIterator<MSADLA.Models.USqlType>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListTypes(account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListTypesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public IEnumerable<MSADLA.Models.USqlTableType> ListTableTypes(AdlClient.Models.AnalyticsAccountRef account, string dbname, string schema)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlTableType>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new PagedIterator<MSADLA.Models.USqlTableType>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListTableTypes(account.Name, dbname, schema, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListTableTypesNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;

        }

        public IEnumerable<MSADLA.Models.USqlTablePartition> ListTablePartitions(AdlClient.Models.AnalyticsAccountRef account, string dbname, string schema, string tablename)
        {
            var pageiter = new PagedIterator<MSADLA.Models.USqlTablePartition>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListTablePartitions(account.Name, dbname, schema, tablename);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListTablePartitionsNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;

        }

        public IEnumerable<MSADLA.Models.USqlTableStatistics> ListTableStatistics(AdlClient.Models.AnalyticsAccountRef account, string dbname, string schema, string tablename)
        {
            var pageiter = new PagedIterator<MSADLA.Models.USqlTableStatistics>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListTableStatistics(account.Name, dbname, schema, tablename);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListTableStatisticsNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

        public void CreateCredential(AdlClient.Models.AnalyticsAccountRef account, string dbname, string credname, MSADLA.Models.DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this.RestClient.Catalog.CreateCredential(account.Name, dbname, credname, create_parameters);
        }

        public void DeleteCredential(AdlClient.Models.AnalyticsAccountRef account, string dbname, string credname, MSADLA.Models.DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this.RestClient.Catalog.DeleteCredential(account.Name, dbname, credname);
        }

        public void UpdateCredential(AdlClient.Models.AnalyticsAccountRef account, string dbname, string credname, MSADLA.Models.DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this.RestClient.Catalog.UpdateCredential(account.Name, dbname, credname, update_parameters);
        }

        public MSADLA.Models.USqlCredential GetCredential(AdlClient.Models.AnalyticsAccountRef account, string dbname, string credname)
        {
            return this.RestClient.Catalog.GetCredential(account.Name, dbname, credname);
        }

        public IEnumerable<MSADLA.Models.USqlCredential> ListCredential(AdlClient.Models.AnalyticsAccountRef account, string dbname)
        {
            var oDataQuery = new MSODATA.ODataQuery<MSADLA.Models.USqlCredential>();
            string select_cols = null;
            bool? count = null;

            var pageiter = new PagedIterator<MSADLA.Models.USqlCredential>();
            pageiter.GetFirstPage = () => this.RestClient.Catalog.ListCredentials(account.Name, dbname, oDataQuery, select_cols, count);
            pageiter.GetNextPage = p => this.RestClient.Catalog.ListCredentialsNext(p.NextPageLink);

            int top = 0;
            var items = pageiter.EnumerateItems(top);
            return items;
        }

    }
}
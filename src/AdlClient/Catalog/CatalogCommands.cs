using System.Collections.Generic;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Catalog
{
    public class CatalogCommands
    {
        private AnalyticsAccountRef account;
        AnalyticsRestClients clients;

        public CatalogCommands(AnalyticsAccountRef a, AnalyticsRestClients c)
        {
            this.account = a;
            this.clients = c;
        }

        public ADL.Analytics.Models.USqlDatabase GetDatabase(string name)
        {
            var db = this.clients._CatalogRest.GetDatabase(this.account, name);
            return db;
        }

        public IEnumerable<ADL.Analytics.Models.USqlDatabase> ListDatabases()
        {
            return this.clients._CatalogRest.ListDatabases(this.account);
        }

        public IEnumerable<ADL.Analytics.Models.USqlAssemblyClr> ListAssemblies(string dbname)
        {
            return this.clients._CatalogRest.ListAssemblies(this.account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(string dbname)
        {
            return this.clients._CatalogRest.ListExternalDatasources(this.account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlProcedure> ListProcedures(string dbname, string schema)
        {
            return this.clients._CatalogRest.ListProcedures(this.account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlSchema> ListSchemas(string dbname)
        {
            return this.clients._CatalogRest.ListSchemas(this.account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlView> ListViews(string dbname, string schema)
        {
            return this.clients._CatalogRest.ListViews(this.account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTable> ListTables(string dbname, string schema)
        {
            return this.clients._CatalogRest.ListTables(this.account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlType> ListTypes(string dbname, string schema)
        {
            return this.clients._CatalogRest.ListTypes(this.account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableType> ListTableTypes(string dbname, string schema)
        {
            return this.clients._CatalogRest.ListTableTypes(this.account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTablePartition> ListTablePartitions(string dbname, string schema, string tablename)
        {
            return this.clients._CatalogRest.ListTablePartitions(this.account, dbname, schema, tablename);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableStatistics> ListTableStatistics(string dbname, string schema, string tablename)
        {
            return this.clients._CatalogRest.ListTableStatistics(this.account, dbname, schema, tablename);
        }

        public void CreateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this.clients._CatalogRest.CreateCredential(this.account, dbname, credname, create_parameters);
        }

        public void DeleteCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this.clients._CatalogRest.DeleteCredential(this.account, dbname, credname, delete_parameters);
        }

        public void UpdateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this.clients._CatalogRest.UpdateCredential(this.account, dbname, credname, update_parameters);
        }

        public ADL.Analytics.Models.USqlCredential GetCredential(string dbname, string credname)
        {
            return this.clients._CatalogRest.GetCredential(this.account, dbname, credname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlCredential> ListCredential(string dbname)
        {
            return this.clients._CatalogRest.ListCredential(this.account, dbname);
        }

    }
}
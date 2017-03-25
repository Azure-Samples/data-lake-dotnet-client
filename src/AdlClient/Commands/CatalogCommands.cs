using System.Collections.Generic;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Commands
{
    public class CatalogCommands
    {
        private AnalyticsAccountRef _account;
        AnalyticsRestClients _restclients;

        public CatalogCommands(AnalyticsAccountRef accout, AnalyticsRestClients restclients)
        {
            this._account = accout;
            this._restclients = restclients;
        }

        public ADL.Analytics.Models.USqlDatabase GetDatabase(string dbname)
        {
            var db = this._restclients._CatalogRest.GetDatabase(this._account, dbname);
            return db;
        }

        public IEnumerable<ADL.Analytics.Models.USqlDatabase> ListDatabases()
        {
            return this._restclients._CatalogRest.ListDatabases(this._account);
        }

        public IEnumerable<ADL.Analytics.Models.USqlAssemblyClr> ListAssemblies(string dbname)
        {
            return this._restclients._CatalogRest.ListAssemblies(this._account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(string dbname)
        {
            return this._restclients._CatalogRest.ListExternalDatasources(this._account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlProcedure> ListProcedures(string dbname, string schema)
        {
            return this._restclients._CatalogRest.ListProcedures(this._account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlSchema> ListSchemas(string dbname)
        {
            return this._restclients._CatalogRest.ListSchemas(this._account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlView> ListViews(string dbname, string schema)
        {
            return this._restclients._CatalogRest.ListViews(this._account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTable> ListTables(string dbname, string schema)
        {
            return this._restclients._CatalogRest.ListTables(this._account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlType> ListTypes(string dbname, string schema)
        {
            return this._restclients._CatalogRest.ListTypes(this._account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableType> ListTableTypes(string dbname, string schema)
        {
            return this._restclients._CatalogRest.ListTableTypes(this._account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTablePartition> ListTablePartitions(string dbname, string schema, string tablename)
        {
            return this._restclients._CatalogRest.ListTablePartitions(this._account, dbname, schema, tablename);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableStatistics> ListTableStatistics(string dbname, string schema, string tablename)
        {
            return this._restclients._CatalogRest.ListTableStatistics(this._account, dbname, schema, tablename);
        }

        public void CreateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this._restclients._CatalogRest.CreateCredential(this._account, dbname, credname, create_parameters);
        }

        public void DeleteCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this._restclients._CatalogRest.DeleteCredential(this._account, dbname, credname, delete_parameters);
        }

        public void UpdateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this._restclients._CatalogRest.UpdateCredential(this._account, dbname, credname, update_parameters);
        }

        public ADL.Analytics.Models.USqlCredential GetCredential(string dbname, string credname)
        {
            return this._restclients._CatalogRest.GetCredential(this._account, dbname, credname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlCredential> ListCredential(string dbname)
        {
            return this._restclients._CatalogRest.ListCredential(this._account, dbname);
        }
    }
}
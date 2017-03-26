using System.Collections.Generic;
using ADL = Microsoft.Azure.Management.DataLake;

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
            var db = this.RestClients._CatalogRest.GetDatabase(this.Account, dbname);
            return db;
        }

        public IEnumerable<ADL.Analytics.Models.USqlDatabase> ListDatabases()
        {
            return this.RestClients._CatalogRest.ListDatabases(this.Account);
        }

        public IEnumerable<ADL.Analytics.Models.USqlAssemblyClr> ListAssemblies(string dbname)
        {
            return this.RestClients._CatalogRest.ListAssemblies(this.Account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlExternalDataSource> ListExternalDatasources(string dbname)
        {
            return this.RestClients._CatalogRest.ListExternalDatasources(this.Account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlProcedure> ListProcedures(string dbname, string schema)
        {
            return this.RestClients._CatalogRest.ListProcedures(this.Account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlSchema> ListSchemas(string dbname)
        {
            return this.RestClients._CatalogRest.ListSchemas(this.Account, dbname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlView> ListViews(string dbname, string schema)
        {
            return this.RestClients._CatalogRest.ListViews(this.Account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTable> ListTables(string dbname, string schema)
        {
            return this.RestClients._CatalogRest.ListTables(this.Account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlType> ListTypes(string dbname, string schema)
        {
            return this.RestClients._CatalogRest.ListTypes(this.Account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableType> ListTableTypes(string dbname, string schema)
        {
            return this.RestClients._CatalogRest.ListTableTypes(this.Account, dbname, schema);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTablePartition> ListTablePartitions(string dbname, string schema, string tablename)
        {
            return this.RestClients._CatalogRest.ListTablePartitions(this.Account, dbname, schema, tablename);
        }

        public IEnumerable<ADL.Analytics.Models.USqlTableStatistics> ListTableStatistics(string dbname, string schema, string tablename)
        {
            return this.RestClients._CatalogRest.ListTableStatistics(this.Account, dbname, schema, tablename);
        }

        public void CreateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialCreateParameters create_parameters)
        {
            this.RestClients._CatalogRest.CreateCredential(this.Account, dbname, credname, create_parameters);
        }

        public void DeleteCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialDeleteParameters delete_parameters)
        {
            this.RestClients._CatalogRest.DeleteCredential(this.Account, dbname, credname, delete_parameters);
        }

        public void UpdateCredential(string dbname, string credname, ADL.Analytics.Models.DataLakeAnalyticsCatalogCredentialUpdateParameters update_parameters)
        {
            this.RestClients._CatalogRest.UpdateCredential(this.Account, dbname, credname, update_parameters);
        }

        public ADL.Analytics.Models.USqlCredential GetCredential(string dbname, string credname)
        {
            return this.RestClients._CatalogRest.GetCredential(this.Account, dbname, credname);
        }

        public IEnumerable<ADL.Analytics.Models.USqlCredential> ListCredential(string dbname)
        {
            return this.RestClients._CatalogRest.ListCredential(this.Account, dbname);
        }
    }
}
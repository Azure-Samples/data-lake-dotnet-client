using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Analytics
{
    [TestClass]
    public class Analytics_Catalog_Tests : Base_Tests
    {
        [TestMethod]
        public void List_Databases()
        {
            this.Initialize();
            foreach (var db in this.AnalyticsClient.Catalog.ListDatabases())
            {
                System.Console.WriteLine("DB {0}",db.Name);
            }
        }

    }

}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests.Store
{
    [TestClass]
    public class Store_Management_Tests : Base_Tests
    {
 

        [TestMethod]
        public void Get_AD_Tenant_ID()
        {
            this.Initialize();
            var directory = AdlClient.Authentication.Directory.Resolve("microsoft.com");
            string tenantid = directory.TenantId;
        }

        [TestMethod]
        public void List_ADLS_Accounts()
        {
            this.Initialize();
            var adls_accounts = this.AzureClient.Store.ListAccounts();
            foreach (var a in adls_accounts)
            {
                System.Console.WriteLine("Store {0} ", a.Name);
            }
        }

    }
}


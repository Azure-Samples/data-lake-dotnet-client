using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    public class Base_Tests
    {
        private bool init;

        public AzureDataLakeClient.Authentication.AuthenticatedSession auth_session;
        public AzureDataLakeClient.Analytics.AnalyticsJobClient adla_job_client;
        public AzureDataLakeClient.Analytics.AnalyticsCatalogClient adla_catalog_client;
        public AzureDataLakeClient.Analytics.AnalyticsManagementClient adla_mgmt_client;
        public AzureDataLakeClient.Store.StoreFileSystemClient adls_fs_client;
        public AzureDataLakeClient.Store.StoreManagementClient adls_mgmt_client;
        public AzureDataLakeClient.Subscription sub;


        public void Initialize()
        {
            if (this.init == false)
            {
                this.auth_session = new AzureDataLakeClient.Authentication.AuthenticatedSession("ADL_Demo_Client");
                auth_session.Authenticate();

                string store_account = "datainsightsadhoc";
                string analytics_account = "datainsightsadhoc";
                string subid = "045c28ea-c686-462f-9081-33c34e871ba3";
                this.sub = new AzureDataLakeClient.Subscription(subid);
                this.init = true;

                this.adls_fs_client = new AzureDataLakeClient.Store.StoreFileSystemClient(store_account, auth_session);
                this.adla_job_client = new AzureDataLakeClient.Analytics.AnalyticsJobClient(analytics_account, auth_session);
                this.adla_catalog_client = new AzureDataLakeClient.Analytics.AnalyticsCatalogClient(analytics_account, auth_session);
                this.adls_mgmt_client = new AzureDataLakeClient.Store.StoreManagementClient(sub, auth_session);
                this.adla_mgmt_client = new AzureDataLakeClient.Analytics.AnalyticsManagementClient(sub, auth_session);

            }
        }

    }
}

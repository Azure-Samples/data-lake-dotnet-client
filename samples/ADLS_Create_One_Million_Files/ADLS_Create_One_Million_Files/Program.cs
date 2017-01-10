using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADLS_Create_One_Million_Files
{
    class Program
    {
        static void Main(string[] args)
        {
            string tenant = "microsoft.onmicrosoft.com"; // change this to YOUR tenant 

            string adls_account = "adltrainingsampledata"; // change this to an ADL Store account you have access to  


            var auth_session = new AzureDataLakeClient.Authentication.AuthenticatedSession("ADL_Demo_Client", tenant);

            auth_session.Authenticate();


            var fs_client = new AzureDataLakeClient.Store.StoreFileSystemClient(adls_account, auth_session);

            var level_a = Enumerable.Range(1, 100);
            var level_b = Enumerable.Range(1, 100);
            var level_c = Enumerable.Range(1, 100);

            int count = 1;

            var opts = new AzureDataLakeClient.Store.CreateFileOptions();
            opts.Overwrite = true;
            foreach (var a in level_a)
            {

                foreach (var b in level_b)
                {
                    foreach (var c in level_c)
                    {
                        string text = string.Format("{0},{1},{2},{3}",a,b,c,count);

                        var p = new AzureDataLakeClient.Store.FsPath(string.Format("/ManyFiles2/{0}/{1}/{2}/data_{3}.csv",a,b,c,count));


                        fs_client.CreateFileWithContent(p, Encoding.UTF8.GetBytes(text),opts);


                        Console.WriteLine("{0} {1}", count, text);

                        count++; 
                    }

                }

            }

        }
    }
}

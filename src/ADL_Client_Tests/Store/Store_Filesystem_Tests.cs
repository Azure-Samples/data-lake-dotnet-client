using AzureDataLakeClient.Store;
using AzureDataLakeClient.Store.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests.Store
{
    [TestClass]
    public class Store_Filesystem_Tests : Base_Tests
    {
        [TestMethod]
        public void List_Files_Recursive()
        {
            this.Initialize();

            int page_count = 0;
            int child_count = 0;

            var lfo = new ListFilesOptions();
            lfo.PageSize = 4;

            var pages = this.adls_account_client.FileSystem.ListFilesRecursivePaged(FsPath.Root, lfo);
            foreach (var page in pages)
            {
                foreach (var child in page.FileItems)
                {
                    child_count++;
                }
                page_count++;

                if (page_count == 3)
                {
                    break;
                }
            }

            Assert.AreEqual(3,page_count);
            Assert.AreEqual(3*4,child_count);
        }

        [TestMethod]
        public void List_Files_Non_Recursive()
        {
            this.Initialize();

            int page_count = 0;
            int child_count = 0;

            var lfo = new ListFilesOptions();
            lfo.PageSize = 4;

            var pages = this.adls_account_client.FileSystem.ListFilesPaged(FsPath.Root, lfo);
            foreach (var page in pages)
            {
                foreach (var fileitem in page.FileItems)
                {
                    child_count++;
                }
                page_count++;

            }

        }

        [TestMethod]
        public void Basic_File_Scenario()
        {
            this.Initialize();
            var dir = create_test_dir();
            
            var fname = dir.Append("foo.txt");
            var cfo = new CreateFileOptions();
            cfo.Overwrite = true;
            this.adls_account_client.FileSystem.Create(fname, "HelloWorld", cfo);
            Assert.IsTrue( this.adls_account_client.FileSystem.Exists(fname));
            var fi = this.adls_account_client.FileSystem.GetFileStatus(fname);
            Assert.AreEqual(10,fi.Length);

            using (var s = this.adls_account_client.FileSystem.OpenText(fname))
            {
                var content = s.ReadToEnd();
                Assert.AreEqual("HelloWorld",content);
            }

            this.adls_account_client.FileSystem.Delete(dir,true);
            Assert.IsFalse(this.adls_account_client.FileSystem.Exists(fname));
            Assert.IsFalse(this.adls_account_client.FileSystem.Exists(dir));

        }

        [TestMethod]
        public void Basic_File_Concatenate_Scenario()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname1 = dir.Append("foo.txt");
            var fname2 = dir.Append("bar.txt");
            var fname3 = dir.Append("beer.txt");

            var cfo = new CreateFileOptions();
            cfo.Overwrite = true;

            this.adls_account_client.FileSystem.Create(fname1, "Hello", cfo);
            this.adls_account_client.FileSystem.Create(fname2, "World", cfo);          
            this.adls_account_client.FileSystem.Concat(new [] { fname1, fname2 },fname3);
            using (var s = this.adls_account_client.FileSystem.OpenText(fname3))
            {
                var content = s.ReadToEnd();
                Assert.AreEqual("HelloWorld", content);
            }

            this.adls_account_client.FileSystem.Delete(dir, true);
            Assert.IsFalse(this.adls_account_client.FileSystem.Exists(fname1));
            Assert.IsFalse(this.adls_account_client.FileSystem.Exists(dir));

        }

        private FsPath create_test_dir()
        {
            var dir = new FsPath("/test_adl_demo_client");

            if (this.adls_account_client.FileSystem.Exists(dir))
            {
                this.adls_account_client.FileSystem.Delete(dir, true);
            }

            this.adls_account_client.FileSystem.CreateDirectory(dir);

            if (!this.adls_account_client.FileSystem.Exists(dir))
            {
                Assert.Fail();
            }
            return dir;
        }

    }
}


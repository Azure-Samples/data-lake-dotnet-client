using AdlClient.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Store
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

            var pages = this.StoreClient.FileSystem.ListFilesRecursivePaged(FsPath.Root, lfo);
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

            var pages = this.StoreClient.FileSystem.ListFilesPaged(FsPath.Root, lfo);
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
            this.StoreClient.FileSystem.Create(fname, "HelloWorld", cfo);
            Assert.IsTrue( this.StoreClient.FileSystem.Exists(fname));
            var fi = this.StoreClient.FileSystem.GetFileStatus(fname);
            Assert.AreEqual(10,fi.Length);

            using (var s = this.StoreClient.FileSystem.OpenText(fname))
            {
                var content = s.ReadToEnd();
                Assert.AreEqual("HelloWorld",content);
            }

            this.StoreClient.FileSystem.Delete(dir,true);
            Assert.IsFalse(this.StoreClient.FileSystem.Exists(fname));
            Assert.IsFalse(this.StoreClient.FileSystem.Exists(dir));

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

            this.StoreClient.FileSystem.Create(fname1, "Hello", cfo);
            this.StoreClient.FileSystem.Create(fname2, "World", cfo);          
            this.StoreClient.FileSystem.Concat(new [] { fname1, fname2 },fname3);
            using (var s = this.StoreClient.FileSystem.OpenText(fname3))
            {
                var content = s.ReadToEnd();
                Assert.AreEqual("HelloWorld", content);
            }

            this.StoreClient.FileSystem.Delete(dir, true);
            Assert.IsFalse(this.StoreClient.FileSystem.Exists(fname1));
            Assert.IsFalse(this.StoreClient.FileSystem.Exists(dir));

        }

        private FsPath create_test_dir()
        {
            var dir = new FsPath("/test_adl_demo_client");

            if (this.StoreClient.FileSystem.Exists(dir))
            {
                this.StoreClient.FileSystem.Delete(dir, true);
            }

            this.StoreClient.FileSystem.CreateDirectory(dir);

            if (!this.StoreClient.FileSystem.Exists(dir))
            {
                Assert.Fail();
            }
            return dir;
        }

    }
}


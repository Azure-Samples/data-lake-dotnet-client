using AdlClient.Models;
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

            var lfo = new FileListParameters();
            lfo.PageSize = 4;

            var folder = new FsPath("/test_adl_demo_client/List_Files_Recursive");

            if (StoreClient.FileSystem.FolderExists(folder))
            {
                StoreClient.FileSystem.Delete(folder, true);
            }

            var bytes = new byte[0];
            var fcp = new AdlClient.Models.FileCreateParameters();
            StoreClient.FileSystem.Create(folder.Append("f1"), bytes, fcp);
            StoreClient.FileSystem.Create(folder.Append("f2"), bytes, fcp);
            StoreClient.FileSystem.Create(folder.Append("a/f3"), bytes, fcp);
            StoreClient.FileSystem.Create(folder.Append("a/f4"), bytes, fcp);
            StoreClient.FileSystem.Create(folder.Append("a/b/f5"), bytes, fcp);
            StoreClient.FileSystem.Create(folder.Append("a/b/f6"), bytes, fcp);
            StoreClient.FileSystem.CreateDirectory(folder.Append("b"));

            var rlfo = new FileListRecursiveParameters();
            rlfo.PageSize = 4;

            var pages = this.StoreClient.FileSystem.ListFilesRecursivePaged(folder, rlfo);
            foreach (var page in pages)
            {
                foreach (var child in page.FileItems)
                {
                    child_count++;
                }
                page_count++;
            }

            Assert.AreEqual(3,page_count);
            Assert.AreEqual(9,child_count);
        }

        [TestMethod]
        public void List_Files_Non_Recursive()
        {
            this.Initialize();

            int page_count = 0;
            int child_count = 0;

            var lfo = new FileListParameters();
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
            var cfo = new FileCreateParameters();
            cfo.Overwrite = true;
            this.StoreClient.FileSystem.Create(fname, "HelloWorld", cfo);
            Assert.IsTrue( this.StoreClient.FileSystem.PathExists(fname));
            var fi = this.StoreClient.FileSystem.GetFileStatus(fname);
            Assert.AreEqual(10,fi.Length);

            using (var s = this.StoreClient.FileSystem.OpenText(fname))
            {
                var content = s.ReadToEnd();
                Assert.AreEqual("HelloWorld",content);
            }

            this.StoreClient.FileSystem.Delete(dir,true);
            Assert.IsFalse(this.StoreClient.FileSystem.PathExists(fname));
            Assert.IsFalse(this.StoreClient.FileSystem.PathExists(dir));

        }

        [TestMethod]
        public void Basic_File_Concatenate_Scenario()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname1 = dir.Append("foo.txt");
            var fname2 = dir.Append("bar.txt");
            var fname3 = dir.Append("beer.txt");

            var cfo = new FileCreateParameters();
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
            Assert.IsFalse(this.StoreClient.FileSystem.PathExists(fname1));
            Assert.IsFalse(this.StoreClient.FileSystem.PathExists(dir));

        }

        private FsPath create_test_dir()
        {
            var dir = new FsPath("/test_adl_demo_client");

            if (this.StoreClient.FileSystem.PathExists(dir))
            {
                this.StoreClient.FileSystem.Delete(dir, true);
            }

            this.StoreClient.FileSystem.CreateDirectory(dir);

            if (!this.StoreClient.FileSystem.PathExists(dir))
            {
                Assert.Fail();
            }
            return dir;
        }

    }
}


using System;
using Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Store
{
    [TestClass]
    public class Store_Filesystem_Expiry_Tests : Base_Tests
    {

        [TestMethod]
        public void File_Expiry_not_set()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname1 = dir.Append("foo.txt");

            var cfo = new FileCreateParameters();
            cfo.Overwrite = true;

            this.StoreClient.FileSystem.Create(fname1, "Hello", cfo);

            var before_fstat1 = this.StoreClient.FileSystem.GetFileStatus(fname1);

            // Having no expiry is the same as having an expiry of the UNix Epoch (1970/1/1)
            var before_exp = before_fstat1.ExpirationTime.Value.ToToDateTimeOffset();
            Assert.AreEqual(FsUnixTime.EpochDateTime, before_exp);
        }

        [TestMethod]
        public void File_Expiry_Relative_to_Now()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname1 = dir.Append("foo.txt");

            var cfo = new FileCreateParameters();
            cfo.Overwrite = true;

            this.StoreClient.FileSystem.Create(fname1, "Hello", cfo);

            var now = System.DateTimeOffset.UtcNow;
            var one_day = new TimeSpan(1,0,0,0);
            this.StoreClient.FileSystem.SetFileExpiryRelativeToNow(fname1, one_day);

            var end_fstat1 = this.StoreClient.FileSystem.GetFileStatus(fname1);
            var end_exp = end_fstat1.ExpirationTime.Value.ToToDateTimeOffset();

            var dif = end_exp - now;
            Assert.AreEqual(1.0, dif.TotalDays, 0.0001);

            this.StoreClient.FileSystem.Delete(dir, true);
            Assert.IsFalse(this.StoreClient.FileSystem.Exists(fname1));
            Assert.IsFalse(this.StoreClient.FileSystem.Exists(dir));
        }

        [TestMethod]
        public void File_Expiry_Absolute()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname1 = dir.Append("foo.txt");

            var cfo = new FileCreateParameters();
            cfo.Overwrite = true;

            this.StoreClient.FileSystem.Create(fname1, "Hello", cfo);

            var now = System.DateTimeOffset.UtcNow;
            var future = now.AddDays(365);
            this.StoreClient.FileSystem.SetFileExpiryAbsolute(fname1, future);

            var end_fstat1 = this.StoreClient.FileSystem.GetFileStatus(fname1);
            var end_exp = end_fstat1.ExpirationTime.Value.ToToDateTimeOffset();

            var dif = end_exp - now;
            Assert.AreEqual(365.0, dif.TotalDays, 0.0001);

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


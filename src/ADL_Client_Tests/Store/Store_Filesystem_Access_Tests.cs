using System.Linq;
using AzureDataLakeClient.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests.Store
{
    [TestClass]
    public class Store_Filesystem_Access_Tests : Base_Tests
    {

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

        [TestMethod]
        public void ACLs_Scenario()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname = dir.Append("foo.txt");
            if (this.adls_account_client.FileSystem.Exists(fname))
            {
                this.adls_account_client.FileSystem.Delete(fname);
            }

            var cfo = new CreateFileOptions();
            cfo.Overwrite = true;
            this.adls_account_client.FileSystem.Create(fname, "HelloWorld", cfo);

            var permissions_before = this.adls_account_client.FileSystem.GetAclStatus(fname);

            Assert.AreEqual(true, permissions_before.OwnerPermission.Value.Read);
            Assert.AreEqual(true, permissions_before.OwnerPermission.Value.Write);
            Assert.AreEqual(true, permissions_before.OwnerPermission.Value.Execute);

            Assert.AreEqual(true, permissions_before.GroupPermission.Value.Read);
            Assert.AreEqual(true, permissions_before.GroupPermission.Value.Write);
            Assert.AreEqual(true, permissions_before.GroupPermission.Value.Execute);

            Assert.AreEqual(false, permissions_before.OtherPermission.Value.Read);
            Assert.AreEqual(false, permissions_before.OtherPermission.Value.Write);
            Assert.AreEqual(false, permissions_before.OtherPermission.Value.Execute);

            var modified_entry = new FsAclEntry( AclType.Other,null, new FsPermission("r-x"));
            this.adls_account_client.FileSystem.ModifyAclEntries(fname, modified_entry);

            var permissions_after = this.adls_account_client.FileSystem.GetAclStatus(fname);

            Assert.AreEqual(true, permissions_after.OwnerPermission.Value.Read);
            Assert.AreEqual(true, permissions_after.OwnerPermission.Value.Write);
            Assert.AreEqual(true, permissions_after.OwnerPermission.Value.Execute);

            Assert.AreEqual(true, permissions_after.GroupPermission.Value.Read);
            Assert.AreEqual(true, permissions_after.GroupPermission.Value.Write);
            Assert.AreEqual(true, permissions_after.GroupPermission.Value.Execute);

            Assert.AreEqual(true, permissions_after.OtherPermission.Value.Read);
            Assert.AreEqual(false, permissions_after.OtherPermission.Value.Write);
            Assert.AreEqual(true, permissions_after.OtherPermission.Value.Execute);
        }

        [TestMethod]
        public void ACLs_Scenario_Find_Entries_With_ReadAccess()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname = dir.Append("foo.txt");
            if (this.adls_account_client.FileSystem.Exists(fname))
            {
                this.adls_account_client.FileSystem.Delete(fname);
            }

            var cfo = new CreateFileOptions();
            cfo.Overwrite = true;
            this.adls_account_client.FileSystem.Create(fname, "HelloWorld", cfo);

            var permissions_before = this.adls_account_client.FileSystem.GetAclStatus(fname);

            // find all the named user entries that have write access
            var entries_before = permissions_before.Entries.Where(e => e.Type == AclType.NamedUser).Where(e=>e.Permission.Value.Write).ToList();
            Assert.IsTrue(entries_before.Count>0);

            // Remove write access for all those entries
            var perms_mask = new FsPermission("r-x");
            var new_acls = entries_before.Select(e => e.AndWith(perms_mask));
            this.adls_account_client.FileSystem.SetAcl(fname, new_acls);
 
            var permissions_after = this.adls_account_client.FileSystem.GetAclStatus(fname);
            // find all the named user entries that have write access
            var entries_after = permissions_after.Entries.Where(e => e.Type == AclType.NamedUser).Where(e => e.Permission.Value.Write).ToList();
            // verify that there are no such entries
            Assert.AreEqual(0, entries_after.Count);
        }

        [TestMethod]
        public void ACLs_Scenario_Removed_Named_users()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname = dir.Append("foo.txt");
            if (this.adls_account_client.FileSystem.Exists(fname))
            {
                this.adls_account_client.FileSystem.Delete(fname);
            }

            var cfo = new CreateFileOptions();
            cfo.Overwrite = true;
            this.adls_account_client.FileSystem.Create(fname, "HelloWorld", cfo);

            var permissions_before = this.adls_account_client.FileSystem.GetAclStatus(fname);

            // copy the entries except for the named users
            var new_entries = permissions_before.Entries.Where(e => e.Type != AclType.NamedUser).ToList();
            this.adls_account_client.FileSystem.SetAcl(fname, new_entries);

            var permissions_after = this.adls_account_client.FileSystem.GetAclStatus(fname);
            // find all the named user entries that have write access
            var entries_after = permissions_after.Entries.Where(e => e.Type == AclType.NamedUser).ToList();
            // verify that there are no such entries
            Assert.AreEqual(0, entries_after.Count);
        }

    }
}


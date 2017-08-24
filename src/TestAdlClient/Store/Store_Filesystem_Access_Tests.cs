using System.Linq;
using AdlClient.Models;
using Microsoft.Azure.Graph.RBAC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAdlClient.Store
{
    [TestClass]
    public class Store_Filesystem_Access_Tests : Base_Tests
    {
        [TestMethod]
        public void GraphLookupUser()
        {
            this.Initialize();
            var u = this.AzureClient.GraphClient.Users.Get("saveenr@microsoft.com");
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

        [TestMethod]
        public void ACLs_Scenario()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname = dir.Append("foo.txt");
            if (this.StoreClient.FileSystem.PathExists(fname))
            {
                this.StoreClient.FileSystem.Delete(fname);
            }

            var cfo = new FileCreateParameters();
            cfo.Overwrite = true;
            this.StoreClient.FileSystem.Create(fname, "HelloWorld", cfo);

            var permissions_before = this.StoreClient.FileSystem.GetAclStatus(fname);

            Assert.AreEqual(true, permissions_before.OwnerPermission.Value.Read);
            Assert.AreEqual(true, permissions_before.OwnerPermission.Value.Write);
            Assert.AreEqual(true, permissions_before.OwnerPermission.Value.Execute);

            Assert.AreEqual(true, permissions_before.GroupPermission.Value.Read);
            Assert.AreEqual(true, permissions_before.GroupPermission.Value.Write);
            Assert.AreEqual(true, permissions_before.GroupPermission.Value.Execute);

            Assert.AreEqual(false, permissions_before.OtherPermission.Value.Read);
            Assert.AreEqual(false, permissions_before.OtherPermission.Value.Write);
            Assert.AreEqual(false, permissions_before.OtherPermission.Value.Execute);

            var modified_entry = new FsAclEntry( FsAclType.Other,null, new FsPermission("r-x"));
            this.StoreClient.FileSystem.ModifyAclEntries(fname, modified_entry);

            var permissions_after = this.StoreClient.FileSystem.GetAclStatus(fname);

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
            if (this.StoreClient.FileSystem.PathExists(fname))
            {
                this.StoreClient.FileSystem.Delete(fname);
            }

            var cfo = new FileCreateParameters();
            cfo.Overwrite = true;
            this.StoreClient.FileSystem.Create(fname, "HelloWorld", cfo);

            var permissions_before = this.StoreClient.FileSystem.GetAclStatus(fname);

            // This test depends on a default acl in the account that gives a user rwx on the access and default acl of all folders and files
            // find all the named user entries that have write access
            var entries_before = permissions_before.Entries.Where(e => e.Type == FsAclType.NamedUser).Where(e=>e.Permission.Value.Write).ToList();
            Assert.IsTrue(entries_before.Count>0);

            // Remove write access for all those entries
            var perms_mask = new FsPermission("r-x");
            var new_acls = entries_before.Select(e => e.AndWith(perms_mask));
            this.StoreClient.FileSystem.ModifyAclEntries(fname, new_acls);
 
            var permissions_after = this.StoreClient.FileSystem.GetAclStatus(fname);
            // find all the named user entries that have write access
            var entries_after = permissions_after.Entries.Where(e => e.Type == FsAclType.NamedUser).Where(e => e.Permission.Value.Write).ToList();
            // verify that there are no such entries
            Assert.AreEqual(0, entries_after.Count);
        }

        [TestMethod]
        public void ACLs_Scenario_Removed_Named_users()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname = dir.Append("foo.txt");
            if (this.StoreClient.FileSystem.PathExists(fname))
            {
                this.StoreClient.FileSystem.Delete(fname);
            }

            var cfo = new FileCreateParameters();
            cfo.Overwrite = true;
            this.StoreClient.FileSystem.Create(fname, "HelloWorld", cfo);

            var permissions_before = this.StoreClient.FileSystem.GetAclStatus(fname);

            // copy the entries except for the named users
            var target_entries = permissions_before.Entries.Where(e => e.Type == FsAclType.NamedUser).ToList();
            target_entries = target_entries.Select(i => i.AndWith(FsPermission.None)).ToList();
            this.StoreClient.FileSystem.RemoveAclEntries(fname, target_entries);

            var permissions_after = this.StoreClient.FileSystem.GetAclStatus(fname);
            // find all the named user entries that have write access
            var entries_after = permissions_after.Entries.Where(e => e.Type == FsAclType.NamedUser).ToList();
            // verify that there are no such entries
            Assert.AreEqual(0, entries_after.Count);
        }

        [TestMethod]
        public void ACLs_Scenario_Add_Named_User()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname = dir.Append( nameof(ACLs_Scenario_Add_Named_User) + ".txt");
            if (this.StoreClient.FileSystem.PathExists(fname))
            {
                this.StoreClient.FileSystem.Delete(fname);
            }

            var cfo = new FileCreateParameters();
            cfo.Overwrite = true;
            this.StoreClient.FileSystem.Create(fname, "HelloWorld", cfo);


            var u = this.AzureClient.GraphClient.Users.Get("mahi@microsoft.com");


            var permissions_before = this.StoreClient.FileSystem.GetAclStatus(fname);
            var acl_entry = new FsAclEntry( FsAclType.NamedUser, u.ObjectId, FsPermission.All);
            this.StoreClient.FileSystem.ModifyAclEntries(fname,acl_entry);

            var permissions_after = this.StoreClient.FileSystem.GetAclStatus(fname);

            var new_entries = permissions_after.Entries.Where(e => e.Type == FsAclType.NamedUser).ToList();
            int count = new_entries.Where(i => i.Name == u.ObjectId).Count();

            Assert.AreEqual(1, count);
        }

    }
}


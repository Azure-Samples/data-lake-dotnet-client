using System.Collections.Generic;
using System.Linq;
using AdlClient.FileSystem;
using Microsoft.Azure.Management.DataLake.Store;
using Microsoft.Azure.Management.DataLake.Store.Models;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AdlClient.Rest
{
    public class StoreFileSystemRestWrapper
    {
        public readonly ADL.Store.DataLakeStoreFileSystemManagementClient RestClient;

        public StoreFileSystemRestWrapper(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this.RestClient = new ADL.Store.DataLakeStoreFileSystemManagementClient(creds);
        }

        public void Mkdirs(StoreAccountRef store, FsPath path)
        {
            var result = RestClient.FileSystem.Mkdirs(store.Name, path.ToString());
        }

        public void Delete(StoreAccountRef store, FsPath path)
        {
            var result = RestClient.FileSystem.Delete(store.Name, path.ToString());
        }

        public void Delete(StoreAccountRef store, FsPath path, bool recursive)
        {
            var result = RestClient.FileSystem.Delete(store.Name, path.ToString(), recursive);
        }

        public void Create(StoreAccountRef store, FsPath path, System.IO.Stream streamContents, CreateFileOptions options)
        {
            RestClient.FileSystem.Create(store.Name, path.ToString(), streamContents, options.Overwrite);
        }

        public FsFileStatus GetFileStatus(StoreAccountRef store, FsPath path)
        {
            var info = RestClient.FileSystem.GetFileStatus(store.Name, path.ToString());
            return new FsFileStatus(info.FileStatus);
        }

        public FsAcl GetAclStatus(StoreAccountRef store, FsPath path)
        {
            var acl_result = this.RestClient.FileSystem.GetAclStatus(store.Name, path.ToString());
            var acl_status = acl_result.AclStatus;

            var fs_acl = new FsAcl(acl_status);

            return fs_acl;
        }

        public void ModifyAclEntries(StoreAccountRef store, FsPath path, FsAclEntry entry)
        {
            this.RestClient.FileSystem.ModifyAclEntries(store.Name, path.ToString(), entry.ToString());
        }

        public void ModifyAclEntries(StoreAccountRef store, FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this.RestClient.FileSystem.ModifyAclEntries(store.Name, path.ToString(), s);
        }

        public void SetAcl(StoreAccountRef store, FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this.RestClient.FileSystem.SetAcl(store.Name, path.ToString(), s);
        }

        public void RemoveAcl(StoreAccountRef store, FsPath path)
        {
            this.RestClient.FileSystem.RemoveAcl(store.Name, path.ToString());
        }

        public void RemoveDefaultAcl(StoreAccountRef store, FsPath path)
        {
            this.RestClient.FileSystem.RemoveDefaultAcl(store.Name, path.ToString());
        }

        public System.IO.Stream Open(StoreAccountRef store, FsPath path)
        {
            return this.RestClient.FileSystem.Open(store.Name, path.ToString());
        }

        public System.IO.Stream Open(StoreAccountRef store, FsPath path, long offset, long bytesToRead)
        {
            return this.RestClient.FileSystem.Open(store.Name, path.ToString(), bytesToRead, offset);
        }

        public void Append(StoreAccountRef store, FsFileStatusPage file, System.IO.Stream steamContents)
        {
            this.RestClient.FileSystem.Append(store.Name, file.ToString(), steamContents);
        }

        public void Concat(StoreAccountRef store, IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            var src_file_strings = src_paths.Select(i => i.ToString()).ToList();
            this.RestClient.FileSystem.Concat(store.Name, dest_path.ToString(), src_file_strings);
        }

        public void SetFileExpiry(StoreAccountRef store, FsPath path, System.DateTimeOffset expiretime)
        {
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this.RestClient.FileSystem.SetFileExpiry(store.Name, path.ToString(), ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryNever(StoreAccountRef store, FsPath path)
        {
            this.RestClient.FileSystem.SetFileExpiry(store.Name, path.ToString(), ExpiryOptionType.NeverExpire, null);
        }

        public void SetFileExpiryAbsolute(StoreAccountRef account, FsPath path, System.DateTimeOffset expiretime)
        {
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this.RestClient.FileSystem.SetFileExpiry(account.Name, path.ToString(), ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryRelativeToNow(StoreAccountRef store, FsPath path, System.TimeSpan timespan)
        {
            this.RestClient.FileSystem.SetFileExpiry(store.Name, path.ToString(), ExpiryOptionType.RelativeToNow, (long)timespan.TotalMilliseconds);
        }

        public void SetFileExpiryRelativeToCreationDate(StoreAccountRef store, FsPath path, System.TimeSpan timespan)
        {
            this.RestClient.FileSystem.SetFileExpiry(store.Name, path.ToString(), ExpiryOptionType.RelativeToCreationDate, (long)timespan.TotalMilliseconds);
        }

        public ContentSummary GetContentSummary(StoreAccountRef store, FsPath path)
        {
            var summary = this.RestClient.FileSystem.GetContentSummary(store.Name, path.ToString());
            return summary.ContentSummary;
        }

        public void SetOwner(StoreAccountRef store, FsPath path, string owner, string group)
        {
            this.RestClient.FileSystem.SetOwner(store.Name, path.ToString(), owner, group);
        }

        public void Move(StoreAccountRef store, FsPath src_path, FsPath dest_path)
        {
            this.RestClient.FileSystem.Rename(store.Name, src_path.ToString(), dest_path.ToString());
        }

        public IEnumerable<FsFileStatusPage> ListFilesPaged(StoreAccountRef store, FsPath path, ListFilesOptions options)
        {
            string after = null;
            while (true)
            {
                var result = RestClient.FileSystem.ListFileStatus(store.Name, path.ToString(), options.PageSize, after);

                if (result.FileStatuses.FileStatus.Count > 0)
                {
                    var page = new FsFileStatusPage();
                    page.Path = path;

                    page.FileItems = result.FileStatuses.FileStatus.Select(i => new FsFileStatus(i)).ToList();
                    yield return page;
                    after = result.FileStatuses.FileStatus[result.FileStatuses.FileStatus.Count - 1].PathSuffix;
                }
                else
                {
                    break;
                }

            }
        }

    }
}
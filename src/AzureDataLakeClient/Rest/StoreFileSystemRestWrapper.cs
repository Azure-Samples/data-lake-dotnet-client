using System.Collections.Generic;
using System.Linq;
using AzureDataLakeClient.Store;
using Microsoft.Azure.Management.DataLake.Store;
using Microsoft.Azure.Management.DataLake.Store.Models;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Rest
{
    public class StoreFileSystemRestWrapper
    {
        public ADL.Store.DataLakeStoreFileSystemManagementClient _adls_filesys_rest_client;
        private Microsoft.Rest.ServiceClientCredentials _creds;

        public StoreFileSystemRestWrapper(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._creds = creds;
            this._adls_filesys_rest_client = new ADL.Store.DataLakeStoreFileSystemManagementClient(this._creds);
        }

        public void Mkdirs(StoreUri store, FsPath path)
        {
            var result = _adls_filesys_rest_client.FileSystem.Mkdirs(store.Name, path.ToString());
        }

        public void Delete(StoreUri store, FsPath path)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(store.Name, path.ToString());
        }

        public void Delete(StoreUri store, FsPath path, bool recursive)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(store.Name, path.ToString(), recursive);
        }

        public void Create(StoreUri store, FsPath path, System.IO.Stream streamContents, CreateFileOptions options)
        {
            _adls_filesys_rest_client.FileSystem.Create(store.Name, path.ToString(), streamContents, options.Overwrite);
        }

        public FsFileStatus GetFileStatus(StoreUri store, FsPath path)
        {
            var info = _adls_filesys_rest_client.FileSystem.GetFileStatus(store.Name, path.ToString());
            return new FsFileStatus(info.FileStatus);
        }

        public FsAcl GetAclStatus(StoreUri store, FsPath path)
        {
            var acl_result = this._adls_filesys_rest_client.FileSystem.GetAclStatus(store.Name, path.ToString());
            var acl_status = acl_result.AclStatus;

            var fs_acl = new FsAcl(acl_status);

            return fs_acl;
        }

        public void ModifyAclEntries(StoreUri store, FsPath path, FsAclEntry entry)
        {
            this._adls_filesys_rest_client.FileSystem.ModifyAclEntries(store.Name, path.ToString(), entry.ToString());
        }

        public void ModifyAclEntries(StoreUri store, FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this._adls_filesys_rest_client.FileSystem.ModifyAclEntries(store.Name, path.ToString(), s);
        }

        public void SetAcl(StoreUri store, FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this._adls_filesys_rest_client.FileSystem.SetAcl(store.Name, path.ToString(), s);
        }

        public void RemoveAcl(StoreUri store, FsPath path)
        {
            this._adls_filesys_rest_client.FileSystem.RemoveAcl(store.Name, path.ToString());
        }

        public void RemoveDefaultAcl(StoreUri store, FsPath path)
        {
            this._adls_filesys_rest_client.FileSystem.RemoveDefaultAcl(store.Name, path.ToString());
        }

        public System.IO.Stream Open(StoreUri store, FsPath path)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(store.Name, path.ToString());
        }

        public System.IO.Stream Open(StoreUri store, FsPath path, long offset, long bytesToRead)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(store.Name, path.ToString(), bytesToRead, offset);
        }

        public void Append(StoreUri store, FsFileStatusPage file, System.IO.Stream steamContents)
        {
            this._adls_filesys_rest_client.FileSystem.Append(store.Name, file.ToString(), steamContents);
        }

        public void Concat(StoreUri store, IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            var src_file_strings = src_paths.Select(i => i.ToString()).ToList();
            this._adls_filesys_rest_client.FileSystem.Concat(store.Name, dest_path.ToString(), src_file_strings);
        }

        public void SetFileExpiry(StoreUri store, FsPath path, System.DateTimeOffset expiretime)
        {
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this._adls_filesys_rest_client.FileSystem.SetFileExpiry(store.Name, path.ToString(), ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryNever(StoreUri store, FsPath path)
        {
            this._adls_filesys_rest_client.FileSystem.SetFileExpiry(store.Name, path.ToString(), ExpiryOptionType.NeverExpire, null);
        }

        public void SetFileExpiryAbsolute(StoreUri account, FsPath path, System.DateTimeOffset expiretime)
        {
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this._adls_filesys_rest_client.FileSystem.SetFileExpiry(account.Name, path.ToString(), ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryRelativeToNow(StoreUri store, FsPath path, System.TimeSpan timespan)
        {
            this._adls_filesys_rest_client.FileSystem.SetFileExpiry(store.Name, path.ToString(), ExpiryOptionType.RelativeToNow, (long)timespan.TotalMilliseconds);
        }

        public void SetFileExpiryRelativeToCreationDate(StoreUri store, FsPath path, System.TimeSpan timespan)
        {
            this._adls_filesys_rest_client.FileSystem.SetFileExpiry(store.Name, path.ToString(), ExpiryOptionType.RelativeToCreationDate, (long)timespan.TotalMilliseconds);
        }

        public ContentSummary GetContentSummary(StoreUri store, FsPath path)
        {
            var summary = this._adls_filesys_rest_client.FileSystem.GetContentSummary(store.Name, path.ToString());
            return summary.ContentSummary;
        }

        public void SetOwner(StoreUri store, FsPath path, string owner, string group)
        {
            this._adls_filesys_rest_client.FileSystem.SetOwner(store.Name, path.ToString(), owner, group);
        }

        public void Move(StoreUri store, FsPath src_path, FsPath dest_path)
        {
            this._adls_filesys_rest_client.FileSystem.Rename(store.Name, src_path.ToString(), dest_path.ToString());
        }

        public IEnumerable<FsFileStatusPage> ListFilesPaged(StoreUri store, FsPath path, ListFilesOptions options)
        {
            string after = null;
            while (true)
            {
                var result = _adls_filesys_rest_client.FileSystem.ListFileStatus(store.Name, path.ToString(), options.PageSize, after);

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
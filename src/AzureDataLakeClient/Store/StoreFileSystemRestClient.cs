using System.Collections.Generic;
using System.Linq;
using ADL = Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store;
using Microsoft.Azure.Management.DataLake.Store.Models;

namespace AzureDataLakeClient.Store
{
    public class StoreFileSystemRestClient
    {
        public ADL.Store.DataLakeStoreFileSystemManagementClient _adls_filesys_rest_client;
        private Microsoft.Rest.ServiceClientCredentials _creds;

        public StoreFileSystemRestClient(Microsoft.Rest.ServiceClientCredentials creds)
        {
            this._creds = creds;
            this._adls_filesys_rest_client = new ADL.Store.DataLakeStoreFileSystemManagementClient(this._creds);
        }

        public void Mkdirs(string account, FsPath path)
        {
            var result = _adls_filesys_rest_client.FileSystem.Mkdirs(account, path.ToString());
        }

        public void Delete(string account, FsPath path)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(account, path.ToString());
        }

        public void Delete(string account, FsPath path, bool recursive)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(account, path.ToString(), recursive);
        }

        public void Create(string account, FsPath path, System.IO.Stream streamContents, CreateFileOptions options)
        {
            _adls_filesys_rest_client.FileSystem.Create(account, path.ToString(), streamContents, options.Overwrite);
        }

        public FsFileStatus GetFileStatus(string account, FsPath path)
        {
            var info = _adls_filesys_rest_client.FileSystem.GetFileStatus(account, path.ToString());
            return new FsFileStatus(info.FileStatus);
        }

        public FsAcl GetAclStatus(string account, FsPath path)
        {
            var acl_result = this._adls_filesys_rest_client.FileSystem.GetAclStatus(account, path.ToString());
            var acl_status = acl_result.AclStatus;

            var fs_acl = new FsAcl(acl_status);

            return fs_acl;
        }

        public void ModifyAclEntries(string account, FsPath path, FsAclEntry entry)
        {
            this._adls_filesys_rest_client.FileSystem.ModifyAclEntries(account, path.ToString(), entry.ToString());
        }

        public void ModifyAclEntries(string account, FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this._adls_filesys_rest_client.FileSystem.ModifyAclEntries(account, path.ToString(), s);
        }

        public void SetAcl(string account, FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this._adls_filesys_rest_client.FileSystem.SetAcl(account, path.ToString(), s);
        }

        public void RemoveAcl(string account, FsPath path)
        {
            this._adls_filesys_rest_client.FileSystem.RemoveAcl(account, path.ToString());
        }

        public void RemoveDefaultAcl(string account, FsPath path)
        {
            this._adls_filesys_rest_client.FileSystem.RemoveDefaultAcl(account, path.ToString());
        }

        public System.IO.Stream Open(string account, FsPath path)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(account, path.ToString());
        }

        public System.IO.Stream Open(string account, FsPath path, long offset, long bytesToRead)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(account, path.ToString(), bytesToRead, offset);
        }

        public void Append(string account, FsFileStatusPage file, System.IO.Stream steamContents)
        {
            this._adls_filesys_rest_client.FileSystem.Append(account, file.ToString(), steamContents);
        }

        public void ConcatConcat(string account, IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            var src_file_strings = src_paths.Select(i => i.ToString()).ToList();
            this._adls_filesys_rest_client.FileSystem.Concat(account, dest_path.ToString(), src_file_strings);
        }

        public void SetFileExpiry(string account, FsPath path, ExpiryOptionType opt, System.DateTimeOffset? expiretime)
        {
            if (expiretime.HasValue)
            {
                var ut = new FsUnixTime(expiretime.Value);
                var unix_time = ut.MillisecondsSinceEpoch;
                this._adls_filesys_rest_client.FileSystem.SetFileExpiry(account, path.ToString(), opt, unix_time);
            }
            else
            {
                this._adls_filesys_rest_client.FileSystem.SetFileExpiry(account, path.ToString(), opt, null);
            }
        }


        public void SetFileExpiryAbsolute(string account, FsPath path, System.DateTimeOffset expiretime)
        {
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this._adls_filesys_rest_client.FileSystem.SetFileExpiry(account, path.ToString(), ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryRelativeToNow(string account, FsPath path, System.TimeSpan timespan)
        {
            this._adls_filesys_rest_client.FileSystem.SetFileExpiry(account, path.ToString(), ExpiryOptionType.RelativeToNow, (long)timespan.TotalMilliseconds);
        }

        public void SetFileExpiryRelativeToCreationDate(string account, FsPath path, System.TimeSpan timespan)
        {
            this._adls_filesys_rest_client.FileSystem.SetFileExpiry(account, path.ToString(), ExpiryOptionType.RelativeToCreationDate, (long)timespan.TotalMilliseconds);
        }

        public ContentSummary GetContentSummary(string account, FsPath path)
        {
            var summary = this._adls_filesys_rest_client.FileSystem.GetContentSummary(account, path.ToString());
            return summary.ContentSummary;
        }

        public void SetOwner(string account, FsPath path, string owner, string group)
        {
            this._adls_filesys_rest_client.FileSystem.SetOwner(account, path.ToString(), owner, group);
        }

        public void Move(string account, FsPath src_path, FsPath dest_path)
        {
            this._adls_filesys_rest_client.FileSystem.Rename(account, src_path.ToString(), dest_path.ToString());
        }

        public IEnumerable<FsFileStatusPage> ListFilesPaged(string account, FsPath path, ListFilesOptions options)
        {
            string after = null;
            while (true)
            {
                var result = _adls_filesys_rest_client.FileSystem.ListFileStatus(account, path.ToString(), options.PageSize, after);

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
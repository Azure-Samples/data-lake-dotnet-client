using System.Collections.Generic;
using System.Linq;
using AdlClient.Models;
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

        public void Mkdirs(AdlClient.Models.StoreAccountRef account, FsPath path)
        {
            var result = RestClient.FileSystem.Mkdirs(account.Name, path.ToString());
        }

        public void Delete(AdlClient.Models.StoreAccountRef account, FsPath path)
        {
            var result = RestClient.FileSystem.Delete(account.Name, path.ToString());
        }

        public void Delete(AdlClient.Models.StoreAccountRef account, FsPath path, bool recursive)
        {
            var result = RestClient.FileSystem.Delete(account.Name, path.ToString(), recursive);
        }

        public void Create(AdlClient.Models.StoreAccountRef account, FsPath path, System.IO.Stream streamContents, FileCreateParameters parameters)
        {
            RestClient.FileSystem.Create(account.Name, path.ToString(), streamContents, parameters.Overwrite);
        }

        public FsFileStatus GetFileStatus(AdlClient.Models.StoreAccountRef account, FsPath path)
        {
            var info = RestClient.FileSystem.GetFileStatus(account.Name, path.ToString());
            return new FsFileStatus(info.FileStatus);
        }

        public FsAcl GetAclStatus(AdlClient.Models.StoreAccountRef account, FsPath path)
        {
            var acl_result = this.RestClient.FileSystem.GetAclStatus(account.Name, path.ToString());
            var acl_status = acl_result.AclStatus;

            var fs_acl = new FsAcl(acl_status);

            return fs_acl;
        }

        public void ModifyAclEntries(AdlClient.Models.StoreAccountRef account, FsPath path, FsAclEntry entry)
        {
            this.RestClient.FileSystem.ModifyAclEntries(account.Name, path.ToString(), entry.ToString());
        }

        public void ModifyAclEntries(AdlClient.Models.StoreAccountRef account, FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this.RestClient.FileSystem.ModifyAclEntries(account.Name, path.ToString(), s);
        }

        public void SetAcl(AdlClient.Models.StoreAccountRef account, FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this.RestClient.FileSystem.SetAcl(account.Name, path.ToString(), s);
        }

        public void RemoveAcl(AdlClient.Models.StoreAccountRef account, FsPath path)
        {
            this.RestClient.FileSystem.RemoveAcl(account.Name, path.ToString());
        }

        public void RemoveDefaultAcl(AdlClient.Models.StoreAccountRef account, FsPath path)
        {
            this.RestClient.FileSystem.RemoveDefaultAcl(account.Name, path.ToString());
        }

        public System.IO.Stream Open(AdlClient.Models.StoreAccountRef account, FsPath path)
        {
            return this.RestClient.FileSystem.Open(account.Name, path.ToString());
        }

        public System.IO.Stream Open(AdlClient.Models.StoreAccountRef account, FsPath path, long offset, long bytesToRead)
        {
            return this.RestClient.FileSystem.Open(account.Name, path.ToString(), bytesToRead, offset);
        }

        public void Append(AdlClient.Models.StoreAccountRef account, FsFileStatusPage file, System.IO.Stream steamContents)
        {
            this.RestClient.FileSystem.Append(account.Name, file.ToString(), steamContents);
        }

        public void Concat(AdlClient.Models.StoreAccountRef account, IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            var src_file_strings = src_paths.Select(i => i.ToString()).ToList();
            this.RestClient.FileSystem.Concat(account.Name, dest_path.ToString(), src_file_strings);
        }

        public void SetFileExpiry(AdlClient.Models.StoreAccountRef account, FsPath path, System.DateTimeOffset expiretime)
        {
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this.RestClient.FileSystem.SetFileExpiry(account.Name, path.ToString(), ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryNever(AdlClient.Models.StoreAccountRef account, FsPath path)
        {
            this.RestClient.FileSystem.SetFileExpiry(account.Name, path.ToString(), ExpiryOptionType.NeverExpire, null);
        }

        public void SetFileExpiryAbsolute(AdlClient.Models.StoreAccountRef account, FsPath path, System.DateTimeOffset expiretime)
        {
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this.RestClient.FileSystem.SetFileExpiry(account.Name, path.ToString(), ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryRelativeToNow(AdlClient.Models.StoreAccountRef account, FsPath path, System.TimeSpan timespan)
        {
            this.RestClient.FileSystem.SetFileExpiry(account.Name, path.ToString(), ExpiryOptionType.RelativeToNow, (long)timespan.TotalMilliseconds);
        }

        public void SetFileExpiryRelativeToCreationDate(AdlClient.Models.StoreAccountRef account, FsPath path, System.TimeSpan timespan)
        {
            this.RestClient.FileSystem.SetFileExpiry(account.Name, path.ToString(), ExpiryOptionType.RelativeToCreationDate, (long)timespan.TotalMilliseconds);
        }

        public ContentSummary GetContentSummary(AdlClient.Models.StoreAccountRef account, FsPath path)
        {
            var summary = this.RestClient.FileSystem.GetContentSummary(account.Name, path.ToString());
            return summary.ContentSummary;
        }

        public void SetOwner(AdlClient.Models.StoreAccountRef account, FsPath path, string owner, string group)
        {
            this.RestClient.FileSystem.SetOwner(account.Name, path.ToString(), owner, group);
        }

        public void Rename(AdlClient.Models.StoreAccountRef account, FsPath src_path, FsPath dest_path)
        {
            this.RestClient.FileSystem.Rename(account.Name, src_path.ToString(), dest_path.ToString());
        }

        public IEnumerable<FsFileStatusPage> ListFilesPaged(AdlClient.Models.StoreAccountRef account, FsPath path, FileListingParameters parameters)
        {
            string after = null;
            while (true)
            {
                var result = RestClient.FileSystem.ListFileStatus(account.Name, path.ToString(), parameters.PageSize, after);

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
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

        public void Mkdirs(FsUri uri)
        {
            var result = RestClient.FileSystem.Mkdirs(uri.Account, uri.Path);
        }

        public void Delete(FsUri uri)
        {
            var result = RestClient.FileSystem.Delete(uri.Account, uri.Path);
        }

        public void Delete(FsUri uri, bool recursive)
        {
            var result = RestClient.FileSystem.Delete(uri.Account, uri.Path, recursive);
        }

        public void Create(FsUri uri, System.IO.Stream streamContents, FileCreateParameters parameters)
        {
            RestClient.FileSystem.Create(uri.Account, uri.Path, streamContents, parameters.Overwrite);
        }

        public FsFileStatus GetFileStatus(FsUri uri)
        {
            var info = RestClient.FileSystem.GetFileStatus(uri.Account, uri.Path);
            return new FsFileStatus(info.FileStatus);
        }

        public FsAcl GetAclStatus(FsUri uri)
        {
            var acl_result = this.RestClient.FileSystem.GetAclStatus(uri.Account, uri.Path);
            var acl_status = acl_result.AclStatus;

            var fs_acl = new FsAcl(acl_status);

            return fs_acl;
        }

        public void ModifyAclEntries(FsUri uri, FsAclEntry entry)
        {
            this.RestClient.FileSystem.ModifyAclEntries(uri.Account, uri.Path, entry.ToString());
        }

        public void ModifyAclEntries(FsUri uri, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this.RestClient.FileSystem.ModifyAclEntries(uri.Account, uri.Path, s);
        }

        public void SetAcl(FsUri uri, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            this.RestClient.FileSystem.SetAcl(uri.Account, uri.Path, s);
        }

        public void RemoveAcl(FsUri uri)
        {
            this.RestClient.FileSystem.RemoveAcl(uri.Account, uri.Path);
        }

        public void RemoveAclEntries(FsUri uri, IEnumerable<FsAclEntry> entries)
        {
            var s = FsAclEntry.EntriesToString(entries);
            s = s.Replace("---", ""); // NOTE: RemoveAclEntries doesn't support --- only empty
            this.RestClient.FileSystem.RemoveAclEntries(uri.Account, uri.Path,s);
        }

        public void RemoveDefaultAcl(FsUri uri)
        {
            this.RestClient.FileSystem.RemoveDefaultAcl(uri.Account, uri.Path);
        }

        public System.IO.Stream Open(FsUri uri)
        {
            return this.RestClient.FileSystem.Open(uri.Account, uri.Path);
        }

        public System.IO.Stream Open(FsUri uri, long offset, long bytesToRead)
        {
            return this.RestClient.FileSystem.Open(uri.Account, uri.Path, bytesToRead, offset);
        }

        public void Upload(FsLocalPath frompath, FsUri topath, int threads, bool resume, bool overwrite, bool uploadasbinary)
        {
            this.RestClient.FileSystem.UploadFile(topath.Account, frompath.ToString(), topath.Path, threads, resume, overwrite, uploadasbinary);
        }

        public void Download(FsUri frompath, FsLocalPath topath, int threads, bool resume, bool overwrite)
        {
            this.RestClient.FileSystem.DownloadFile( frompath.Account, frompath.Path, topath.ToString(), threads, resume, overwrite );
        }

        public void Append(FsUri uri, System.IO.Stream steamContents)
        {
            this.RestClient.FileSystem.Append(uri.Account, uri.Path, steamContents);
        }

        public void Concat(AdlClient.Models.StoreAccountRef account, IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            var src_file_strings = src_paths.Select(i => i.ToString()).ToList();
            this.RestClient.FileSystem.Concat(account.Name, dest_path.ToString(), src_file_strings);
        }

        public void SetFileExpiry(FsUri uri, System.DateTimeOffset expiretime)
        {
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this.RestClient.FileSystem.SetFileExpiry(uri.Account, uri.Path, ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryNever(FsUri uri)
        {
            this.RestClient.FileSystem.SetFileExpiry(uri.Account, uri.Path, ExpiryOptionType.NeverExpire, null);
        }

        public void SetFileExpiryAbsolute(FsUri uri, System.DateTimeOffset expiretime)
        {
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this.RestClient.FileSystem.SetFileExpiry(uri.Account, uri.Path, ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryRelativeToNow(FsUri uri, System.TimeSpan timespan)
        {
            this.RestClient.FileSystem.SetFileExpiry(uri.Account, uri.Path, ExpiryOptionType.RelativeToNow, (long)timespan.TotalMilliseconds);
        }

        public void SetFileExpiryRelativeToCreationDate(FsUri uri, System.TimeSpan timespan)
        {
            this.RestClient.FileSystem.SetFileExpiry(uri.Account, uri.Path, ExpiryOptionType.RelativeToCreationDate, (long)timespan.TotalMilliseconds);
        }

        public ContentSummary GetContentSummary(FsUri uri)
        {
            var summary = this.RestClient.FileSystem.GetContentSummary(uri.Account, uri.Path);
            return summary.ContentSummary;
        }

        public void SetOwner(FsUri uri, string owner, string group)
        {
            this.RestClient.FileSystem.SetOwner(uri.Account, uri.Path, owner, group);
        }

        public void Rename(string account, FsPath src_path, FsPath dest_path)
        {
            this.RestClient.FileSystem.Rename(account, src_path.ToString(), dest_path.ToString());
        }

        public IEnumerable<FsFileStatusPage> ListFilesPaged(FsUri uri, FileListingParameters parameters)
        {
            string after = null;
            while (true)
            {
                var result = RestClient.FileSystem.ListFileStatus(uri.Account, uri.Path, parameters.PageSize, after);

                if (result.FileStatuses.FileStatus.Count > 0)
                {
                    var page = new FsFileStatusPage();
                    page.Path = new FsPath(uri.Path);

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
using System.Collections.Generic;
using AdlClient.Models;
using Microsoft.Azure.Management.DataLake.Store;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;
using System.Linq;

namespace AdlClient.Commands
{
    public class FileSystemCommands 
    {
        internal readonly AdlClient.Models.StoreAccountRef Account;
        internal readonly AdlClient.Rest.StoreRestClients RestClients;

        internal FileSystemCommands(AdlClient.Models.StoreAccountRef account, AdlClient.Rest.StoreRestClients restclients)
        {
            this.Account = account;
            this.RestClients = restclients;
        }

        public IEnumerable<FsFileStatusPage> ListFilesRecursivePaged(FsPath path, FileListingParameters parameters)
        {
            var queue = new Queue<FsPath>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                FsPath cur_path = queue.Dequeue();

                foreach (var page in ListFilesPaged(cur_path, parameters))
                {
                    yield return page;

                    foreach (var item in page.FileItems)
                    {
                        if (item.Type == MSADLS.Models.FileType.DIRECTORY)
                        {
                            var new_path = cur_path.Append(item.PathSuffix);
                            queue.Enqueue(new_path);
                        }
                    }
                }
            }
        }

        public IEnumerable<FsFileStatusPage> ListFilesPaged(FsPath path, FileListingParameters parameters)
        {
            var uri = this.GetUri(path);
            string after = null;
            while (true)
            {
                var result = RestClients.FileSystemClient.FileSystem.ListFileStatus(uri.Account, uri.Path, parameters.PageSize, after);

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

        public AdlClient.Models.FsUri GetUri(FsPath path)
        {
            return new AdlClient.Models.FsUri(this.Account.Name, path.ToString());
        }

        public void CreateDirectory(FsPath path)
        {
            var uri = this.GetUri(path);
            var result = this.RestClients.FileSystemClient.FileSystem.Mkdirs(uri.Account, uri.Path);
        }

        public void Delete(FsPath path)
        {
            var uri = this.GetUri(path);
            var result = this.RestClients.FileSystemClient.FileSystem.Delete(uri.Account, uri.Path);
        }

        public void Delete(FsPath path, bool recursive)
        {
            var uri = this.GetUri(path);
            var result = this.RestClients.FileSystemClient.FileSystem.Delete(uri.Account, uri.Path, recursive);
        }

        public void Create(FsPath path, byte[] bytes, FileCreateParameters parameters)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            var uri = this.GetUri(path);
            RestClients.FileSystemClient.FileSystem.Create(uri.Account, uri.Path, memstream, parameters.Overwrite);

        }

        public void Create(FsPath path, string content, FileCreateParameters parameters)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.Create(path, bytes, parameters);
        }

        public FsFileStatus GetFileStatus(FsPath path)
        {
            var uri = this.GetUri(path);
            var info = RestClients.FileSystemClient.FileSystem.GetFileStatus(uri.Account, uri.Path);
            return new FsFileStatus(info.FileStatus);
        }

        public FsFileStatus TryGetFileInformation(FsPath path)
        {
            try
            {
                var info = this.GetFileStatus(path);
                return info;
            }
            catch (MSADLS.Models.AdlsErrorException ex)
            {
                if (ex.Body.RemoteException is MSADLS.Models.AdlsFileNotFoundException ||
                    ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }

        public bool PathExists(FsPath path)
        {
            var info = this.TryGetFileInformation(path);
            return (info != null);
        }

        public bool FileExists(FsPath path)
        {
            var filestat = this.TryGetFileInformation(path);
            if (filestat == null)
            {
                return false;
            }

            if (filestat.Type == MSADLS.Models.FileType.DIRECTORY)
            {
                return false;
            }

            return true;

        }

        public bool FolderExists(FsPath path)
        {
            var info = this.TryGetFileInformation(path);
            if (info == null)
            {
                return false;
            }


            if (info.Type == MSADLS.Models.FileType.FILE)
            {
                return false;
            }

            return true;

        }


        public FsAcl GetAclStatus(FsPath path)
        {
            var uri = this.GetUri(path);
            var acl_result = this.RestClients.FileSystemClient.FileSystem.GetAclStatus(uri.Account, uri.Path);
            var acl_status = acl_result.AclStatus;

            var fs_acl = new FsAcl(acl_status);

            return fs_acl;
        }

        public void ModifyAclEntries(FsPath path, FsAclEntry entry)
        {
            var uri = this.GetUri(path);
            this.RestClients.FileSystemClient.FileSystem.ModifyAclEntries(uri.Account, uri.Path, entry.ToString());
        }

        public void ModifyAclEntries(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var uri = this.GetUri(path);
            var s = FsAclEntry.EntriesToString(entries);
            this.RestClients.FileSystemClient.FileSystem.ModifyAclEntries(uri.Account, uri.Path, s);
        }

        public void SetAcl(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var uri = this.GetUri(path);
            var s = FsAclEntry.EntriesToString(entries);
            this.RestClients.FileSystemClient.FileSystem.SetAcl(uri.Account, uri.Path, s);
        }

        public void RemoveAcl(FsPath path)
        {
            var uri = this.GetUri(path);
            this.RestClients.FileSystemClient.FileSystem.RemoveAcl(uri.Account, uri.Path);
        }

        public void RemoveAclEntries(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            var uri = this.GetUri(path);
            foreach (var entry in entries)
            {
                if (entry.Permission.HasValue)
                {
                    var perm = entry.Permission.Value;

                    if (perm.Integer > 0)
                    {
                        throw new System.ArgumentOutOfRangeException("For RemoveAclEntries the RWX must be empty");
                    }
                }
            }
            var s = FsAclEntry.EntriesToString(entries);
            s = s.Replace("---", ""); // NOTE: RemoveAclEntries doesn't support --- only empty
            this.RestClients.FileSystemClient.FileSystem.RemoveAclEntries(uri.Account, uri.Path, s);
        }

        public void RemoveDefaultAcl(FsPath path)
        {
            var uri = this.GetUri(path);
            this.RestClients.FileSystemClient.FileSystem.RemoveDefaultAcl(uri.Account, uri.Path);
        }

        public System.IO.Stream Open(FsPath path)
        {
            var uri = this.GetUri(path);
            return this.RestClients.FileSystemClient.FileSystem.Open(uri.Account, uri.Path);
        }

        public System.IO.StreamReader OpenText(FsPath path)
        {
            var uri = this.GetUri(path);
            var s = this.RestClients.FileSystemClient.FileSystem.Open(uri.Account, uri.Path);
            return new System.IO.StreamReader(s);
        }

        public System.IO.Stream Open(FsPath path, long offset, long bytesToRead)
        {
            var uri = this.GetUri(path);
            return this.RestClients.FileSystemClient.FileSystem.Open(uri.Account, uri.Path, bytesToRead, offset);
        }

        public void Upload(FsLocalPath src_path, FsPath dest_path, FileUploadParameters parameters)
        {
            var dest_uri = this.GetUri(dest_path);
            this.RestClients.FileSystemClient.FileSystem.UploadFile(
                dest_uri.Account,
                src_path.ToString(), 
                dest_uri.Path, parameters.NumThreads, parameters.Resume, parameters.Overwrite, parameters.UploadAsBinary);
        }

        public void Download(FsPath src_path, FsLocalPath dest_path, FileDownloadParameters parameters)
        {
            var src_uri = this.GetUri(src_path);
            this.RestClients.FileSystemClient.FileSystem.DownloadFile(src_uri.Account, src_uri.Path, dest_path.ToString(), parameters.NumThreads, parameters.Resume, parameters.Overwrite);
        }

        public void Append(FsPath path, string content)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            var uri = this.GetUri(path);
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this.RestClients.FileSystemClient.FileSystem.Append(uri.Account, uri.Path, stream);
            }
        }

        public void Append(FsPath path, byte[] bytes)
        {
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                var uri = this.GetUri(path);
                this.RestClients.FileSystemClient.FileSystem.Append(this.Account.Name, uri.Path, stream);
            }
        }

        public void Concat(IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            var src_file_strings = src_paths.Select(i => i.ToString()).ToList();
            this.RestClients.FileSystemClient.FileSystem.Concat(this.Account.Name, dest_path.ToString(), src_file_strings);

        }

        public void ClearFileExpiry(FsPath path)
        {
            var uri = this.GetUri(path);
            this.RestClients.FileSystemClient.FileSystem.SetFileExpiry(uri.Account, uri.Path, MSADLS.Models.ExpiryOptionType.NeverExpire, null);

        }

        public void SetFileExpiryAbsolute(FsPath path, System.DateTimeOffset expiretime)
        {
            var uri = this.GetUri(path);
            var ut = new FsUnixTime(expiretime);
            var unix_time = ut.MillisecondsSinceEpoch;
            this.RestClients.FileSystemClient.FileSystem.SetFileExpiry(uri.Account, uri.Path, MSADLS.Models.ExpiryOptionType.Absolute, unix_time);
        }

        public void SetFileExpiryRelativeToNow(FsPath path, System.TimeSpan timespan)
        {
            var uri = this.GetUri(path);
            this.RestClients.FileSystemClient.FileSystem.SetFileExpiry(uri.Account, uri.Path, MSADLS.Models.ExpiryOptionType.RelativeToNow, (long)timespan.TotalMilliseconds);
        }

        public void SetFileExpiryRelativeToCreationDate(FsPath path, System.TimeSpan timespan)
        {
            var uri = this.GetUri(path);
            this.RestClients.FileSystemClient.FileSystem.SetFileExpiry(uri.Account, uri.Path, MSADLS.Models.ExpiryOptionType.RelativeToCreationDate, (long)timespan.TotalMilliseconds);
        }

        public MSADLS.Models.ContentSummary GetContentSummary(FsPath path)
        {
            var uri = this.GetUri(path);
            var summary = this.RestClients.FileSystemClient.FileSystem.GetContentSummary(uri.Account, uri.Path);
            return summary.ContentSummary;
        }

        public void SetOwner(FsPath path, string user, string group)
        {
            var uri = this.GetUri(path);
            this.RestClients.FileSystemClient.FileSystem.SetOwner(uri.Account, uri.Path, user, group);
        }

        public void Move(FsPath src_path, FsPath dest_path)
        {
            this.RestClients.FileSystemClient.FileSystem.Rename(this.Account.Name, src_path.ToString(), dest_path.ToString());
        }
    }
}
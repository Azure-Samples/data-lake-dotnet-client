using System.Collections.Generic;
using AdlClient.FileSystem;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;

namespace AdlClient.Commands
{
    public class FileSystemCommands 
    {
        public readonly StoreRestClients RestClients;
        private readonly StoreAccountRef StoreAccount;

        public FileSystemCommands(StoreAccountRef account,StoreRestClients restclients)
        {
            this.StoreAccount = account;
            this.RestClients = restclients;
        }

        public IEnumerable<FsFileStatusPage> ListFilesRecursivePaged(FsPath path, ListFilesOptions options)
        {
            var queue = new Queue<FsPath>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                FsPath cur_path = queue.Dequeue();

                foreach (var page in ListFilesPaged(cur_path, options))
                {
                    yield return page;

                    foreach (var item in page.FileItems)
                    {
                        if (item.Type == Microsoft.Azure.Management.DataLake.Store.Models.FileType.DIRECTORY)
                        {
                            var new_path = cur_path.Append(item.PathSuffix);
                            queue.Enqueue(new_path);
                        }
                    }
                }
            }
        }

        public IEnumerable<FsFileStatusPage> ListFilesPaged(FsPath path, ListFilesOptions options)
        {
            return this.RestClients.FileSystemRest.ListFilesPaged(this.StoreAccount, path, options);
        }

        public void CreateDirectory(FsPath path)
        {
            this.RestClients.FileSystemRest.Mkdirs(this.StoreAccount, path);
        }

        public void Delete(FsPath path)
        {
            RestClients.FileSystemRest.Delete(this.StoreAccount, path);
        }

        public void Delete(FsPath path, bool recursive)
        {
            RestClients.FileSystemRest.Delete(this.StoreAccount, path, recursive);
        }

        public void Create(FsPath path, byte[] bytes, CreateFileOptions options)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            RestClients.FileSystemRest.Create(this.StoreAccount, path, memstream, options);
        }

        public void Create(FsPath path, string content, CreateFileOptions options)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.Create(path, bytes, options);
        }

        public FsFileStatus GetFileStatus(FsPath path)
        {
            return RestClients.FileSystemRest.GetFileStatus(this.StoreAccount, path);
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
                if (ex.Body.RemoteException is Microsoft.Azure.Management.DataLake.Store.Models.AdlsFileNotFoundException ||
                    ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }

        public bool Exists(FsPath path)
        {
            var info = this.TryGetFileInformation(path);
            return (info != null);
        }

        public bool ExistsFile(FsPath path)
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

        public bool ExistsFolder(FsPath path)
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
            var acl_result = RestClients.FileSystemRest.GetAclStatus(this.StoreAccount, path);
            return acl_result;
        }

        public void ModifyAclEntries(FsPath path, FsAclEntry entry)
        {
            this.RestClients.FileSystemRest.ModifyAclEntries(this.StoreAccount, path, entry);
        }

        public void ModifyAclEntries(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this.RestClients.FileSystemRest.ModifyAclEntries(this.StoreAccount, path, entries);
        }

        public void SetAcl(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this.RestClients.FileSystemRest.SetAcl(this.StoreAccount, path, entries);
        }

        public void RemoveAcl(FsPath path)
        {
            this.RestClients.FileSystemRest.RemoveAcl(this.StoreAccount, path);
        }

        public void RemoveDefaultAcl(FsPath path)
        {
            this.RestClients.FileSystemRest.RemoveDefaultAcl(this.StoreAccount, path);
        }

        public System.IO.Stream Open(FsPath path)
        {
            return this.RestClients.FileSystemRest.Open(this.StoreAccount, path);
        }

        public System.IO.StreamReader OpenText(FsPath path)
        {
            var s = this.RestClients.FileSystemRest.Open(this.StoreAccount, path);
            return new System.IO.StreamReader(s);
        }

        public System.IO.Stream Open(FsPath path, long offset, long bytesToRead)
        {
            return this.RestClients.FileSystemRest.Open(this.StoreAccount, path, bytesToRead, offset);
        }

        public void Upload(LocalPath src_path, FsPath dest_path, UploadOptions options)
        {
            var parameters = new Microsoft.Azure.Management.DataLake.StoreUploader.UploadParameters(src_path.ToString(), dest_path.ToString(), this.StoreAccount.Name, isOverwrite: options.Force);
            var frontend = new Microsoft.Azure.Management.DataLake.StoreUploader.DataLakeStoreFrontEndAdapter(this.StoreAccount.Name, this.RestClients.FileSystemRest.RestClient);
            var uploader = new Microsoft.Azure.Management.DataLake.StoreUploader.DataLakeStoreUploader(parameters, frontend);
            uploader.Execute();
        }

        public void Download(FsPath src_path, LocalPath dest_path, DownloadOptions options)
        {
            using (var stream = this.RestClients.FileSystemRest.Open(this.StoreAccount, src_path))
            {
                var filemode = options.Append ? System.IO.FileMode.Append : System.IO.FileMode.Create;
                using (var fileStream = new System.IO.FileStream(dest_path.ToString(), filemode))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public void Appen(FsFileStatusPage file, string content)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this.RestClients.FileSystemRest.Append(this.StoreAccount, file, stream);
            }
        }

        public void Append(FsFileStatusPage file, byte[] bytes)
        {
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this.RestClients.FileSystemRest.Append(this.StoreAccount, file, stream);
            }
        }

        public void Concat(IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            this.RestClients.FileSystemRest.Concat(this.StoreAccount, src_paths, dest_path);
        }

        public void ClearFileExpiry(FsPath path)
        {
            this.RestClients.FileSystemRest.SetFileExpiryNever(this.StoreAccount, path);
        }

        public void SetFileExpiryAbsolute(FsPath path, System.DateTimeOffset expiretime)
        {
            this.RestClients.FileSystemRest.SetFileExpiry(this.StoreAccount, path, expiretime);
        }

        public void SetFileExpiryRelativeToNow(FsPath path, System.TimeSpan timespan)
        {
            this.RestClients.FileSystemRest.SetFileExpiryRelativeToNow(this.StoreAccount, path, timespan);
        }

        public void SetFileExpiryRelativeToCreationDate(FsPath path, System.TimeSpan timespan)
        {
            this.RestClients.FileSystemRest.SetFileExpiryRelativeToCreationDate(this.StoreAccount, path, timespan);
        }

        public MSADLS.Models.ContentSummary GetContentSummary(FsPath path)
        {
            return this.RestClients.FileSystemRest.GetContentSummary(this.StoreAccount, path);
        }

        public void SetOwner(FsPath path, string user, string group)
        {
            this.RestClients.FileSystemRest.SetOwner(this.StoreAccount, path, user, group);
        }

        public void Move(FsPath src_path, FsPath dest_path)
        {
            this.RestClients.FileSystemRest.Move(this.StoreAccount, src_path, dest_path);
        }
    }
}
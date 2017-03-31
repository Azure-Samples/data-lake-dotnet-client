using System.Collections.Generic;
using AdlClient.Models;
using MSADLS = Microsoft.Azure.Management.DataLake.Store;

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
            return this.RestClients.FileSystemRest.ListFilesPaged(this.GetUri(path), parameters);
        }

        public AdlClient.Models.FsUri GetUri(FsPath path)
        {
            return new AdlClient.Models.FsUri(this.Account.Name, path.ToString());
        }

        public void CreateDirectory(FsPath path)
        {
            this.RestClients.FileSystemRest.Mkdirs( this.GetUri(path) );
        }

        public void Delete(FsPath path)
        {
            RestClients.FileSystemRest.Delete( this.GetUri(path) );
        }

        public void Delete(FsPath path, bool recursive)
        {
            RestClients.FileSystemRest.Delete(this.GetUri(path), recursive);
        }

        public void Create(FsPath path, byte[] bytes, FileCreateParameters parameters)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            RestClients.FileSystemRest.Create(this.GetUri(path), memstream, parameters);
        }

        public void Create(FsPath path, string content, FileCreateParameters parameters)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.Create(path, bytes, parameters);
        }

        public FsFileStatus GetFileStatus(FsPath path)
        {
            return RestClients.FileSystemRest.GetFileStatus(this.GetUri(path));
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
            var acl_result = RestClients.FileSystemRest.GetAclStatus(this.GetUri(path));
            return acl_result;
        }

        public void ModifyAclEntries(FsPath path, FsAclEntry entry)
        {
            this.RestClients.FileSystemRest.ModifyAclEntries(this.GetUri(path),entry);
        }

        public void ModifyAclEntries(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this.RestClients.FileSystemRest.ModifyAclEntries(this.GetUri(path), entries);
        }

        public void SetAcl(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this.RestClients.FileSystemRest.SetAcl(this.GetUri(path), entries);
        }

        public void RemoveAcl(FsPath path)
        {
            this.RestClients.FileSystemRest.RemoveAcl(this.GetUri(path));
        }

        public void RemoveDefaultAcl(FsPath path)
        {
            this.RestClients.FileSystemRest.RemoveDefaultAcl(this.GetUri(path));
        }

        public System.IO.Stream Open(FsPath path)
        {
            return this.RestClients.FileSystemRest.Open(this.GetUri(path));
        }

        public System.IO.StreamReader OpenText(FsPath path)
        {
            var s = this.RestClients.FileSystemRest.Open(this.GetUri(path));
            return new System.IO.StreamReader(s);
        }

        public System.IO.Stream Open(FsPath path, long offset, long bytesToRead)
        {
            return this.RestClients.FileSystemRest.Open(this.GetUri(path), bytesToRead, offset);
        }

        public void Upload(FsLocalPath src_path, FsPath dest_path, FileUploadParameters parameters)
        {
            var uploader_parameters = new Microsoft.Azure.Management.DataLake.StoreUploader.UploadParameters(src_path.ToString(), dest_path.ToString(), this.Account.Name, isOverwrite: parameters.Force);
            var frontend = new Microsoft.Azure.Management.DataLake.StoreUploader.DataLakeStoreFrontEndAdapter(this.Account.Name, this.RestClients.FileSystemRest.RestClient);
            var uploader = new Microsoft.Azure.Management.DataLake.StoreUploader.DataLakeStoreUploader(uploader_parameters, frontend);
            uploader.Execute();
        }

        public void Download(FsPath src_path, FsLocalPath dest_path, FileDownloadParameters parameters)
        {
            using (var stream = this.RestClients.FileSystemRest.Open(this.GetUri(src_path)))
            {
                var filemode = parameters.Append ? System.IO.FileMode.Append : System.IO.FileMode.Create;
                using (var fileStream = new System.IO.FileStream(dest_path.ToString(), filemode))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public void Append(FsPath path, string content)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this.RestClients.FileSystemRest.Append(this.GetUri(path), stream);
            }
        }

        public void Append(FsPath path, byte[] bytes)
        {
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this.RestClients.FileSystemRest.Append(this.GetUri(path), stream);
            }
        }

        public void Concat(IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            this.RestClients.FileSystemRest.Concat(this.Account, src_paths, dest_path);
        }

        public void ClearFileExpiry(FsPath path)
        {
            this.RestClients.FileSystemRest.SetFileExpiryNever(this.GetUri(path));
        }

        public void SetFileExpiryAbsolute(FsPath path, System.DateTimeOffset expiretime)
        {
            this.RestClients.FileSystemRest.SetFileExpiry(this.GetUri(path), expiretime);
        }

        public void SetFileExpiryRelativeToNow(FsPath path, System.TimeSpan timespan)
        {
            this.RestClients.FileSystemRest.SetFileExpiryRelativeToNow(this.GetUri(path), timespan);
        }

        public void SetFileExpiryRelativeToCreationDate(FsPath path, System.TimeSpan timespan)
        {
            this.RestClients.FileSystemRest.SetFileExpiryRelativeToCreationDate(this.GetUri(path), timespan);
        }

        public MSADLS.Models.ContentSummary GetContentSummary(FsPath path)
        {
            return this.RestClients.FileSystemRest.GetContentSummary(this.GetUri(path));
        }

        public void SetOwner(FsPath path, string user, string group)
        {
            this.RestClients.FileSystemRest.SetOwner(this.GetUri(path), user, group);
        }

        public void Move(FsPath src_path, FsPath dest_path)
        {
            this.RestClients.FileSystemRest.Rename(this.Account.Name, src_path, dest_path);
        }
    }
}
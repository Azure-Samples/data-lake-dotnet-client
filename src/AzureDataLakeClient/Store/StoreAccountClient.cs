using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.Store.Clients;
using Microsoft.Azure.Management.DataLake.Store.Models;
using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLakeClient.Store
{
    public class StoreAccountClient : AccountClientBase
    {
        private StoreFileSystemRestClient _fs_rest_client;
        private StoreManagementRestClient _mgmt_rest_client;
        private StoreAccount _store;
        public StoreAccountClient(StoreAccount store, AuthenticatedSession authSession) :
            base(store.Name,authSession)
        {
            _fs_rest_client = new StoreFileSystemRestClient(this.AuthenticatedSession.Credentials);
            this._store = store;
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
                        if (item.Type == ADL.Store.Models.FileType.DIRECTORY)
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
            return this._fs_rest_client.ListFilesPaged(this._store.GetUri(), path, options);
        }
        
        public void CreateDirectory(FsPath path)
        {
            this._fs_rest_client.Mkdirs(this._store.GetUri(), path);
        }

        public void Delete(FsPath path)
        {
            _fs_rest_client.Delete(this._store.GetUri(), path);
        }

        public void Delete(FsPath path, bool recursive)
        {
            _fs_rest_client.Delete(this._store.GetUri(), path, recursive );
        }

        public void Create(FsPath path, byte[] bytes, CreateFileOptions options)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            _fs_rest_client.Create(this._store.GetUri(), path,memstream, options);
        }

        public void Create(FsPath path, string content, CreateFileOptions options)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.Create(path, bytes, options);
        }

        public FsFileStatus GetFileStatus(FsPath path)
        {
            return this._fs_rest_client.GetFileStatus(this._store.GetUri(), path);
        }

        public FsFileStatus TryGetFileInformation(FsPath path)
        {
            try
            {
                var info = this.GetFileStatus(path);
                return info;
            }
            catch (Microsoft.Azure.Management.DataLake.Store.Models.AdlsErrorException ex)
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

            if (filestat.Type == FileType.DIRECTORY)
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


            if (info.Type == FileType.FILE)
            {
                return false;
            }

            return true;

        }


        public FsAcl GetAclStatus(FsPath path)
        {
            var acl_result = this._fs_rest_client.GetAclStatus(this._store.GetUri(), path);
            return acl_result;
        }

        public void ModifyAclEntries(FsPath path, FsAclEntry entry)
        {
            this._fs_rest_client.ModifyAclEntries(this._store.GetUri(), path, entry);
        }

        public void ModifyAclEntries(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this._fs_rest_client.ModifyAclEntries(this._store.GetUri(), path, entries);
        }

        public void SetAcl(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this._fs_rest_client.SetAcl(this._store.GetUri(), path, entries);
        }

        public void RemoveAcl(FsPath path)
        {
            this._fs_rest_client.RemoveAcl(this._store.GetUri(), path);
        }

        public void RemoveDefaultAcl(FsPath path)
        {
            this._fs_rest_client.RemoveDefaultAcl(this._store.GetUri(), path);
        }

        public System.IO.Stream Open(FsPath path)
        {
            return this._fs_rest_client.Open(this._store.GetUri(), path);
        }

        public System.IO.StreamReader OpenText(FsPath path)
        {
            var s = this._fs_rest_client.Open(this._store.GetUri(), path);
            return new System.IO.StreamReader(s);
        }

        public System.IO.Stream Open(FsPath path, long offset, long bytesToRead)
        {
            return this._fs_rest_client.Open(this._store.GetUri(), path, bytesToRead, offset);
        }

        public void Upload(LocalPath src_path, FsPath dest_path, UploadOptions options)
        {
            var parameters = new ADL.StoreUploader.UploadParameters(src_path.ToString(), dest_path.ToString(), this.Account, isOverwrite: options.Force);
            var frontend = new ADL.StoreUploader.DataLakeStoreFrontEndAdapter(this.Account, this._fs_rest_client._adls_filesys_rest_client);
            var uploader = new ADL.StoreUploader.DataLakeStoreUploader(parameters, frontend);
            uploader.Execute();
        }

        public void Download(FsPath src_path, LocalPath dest_path, DownloadOptions options)
        {
            using (var stream = this._fs_rest_client.Open(this._store.GetUri(),src_path))
            {
                var filemode = options.Append ? System.IO.FileMode.Append : System.IO.FileMode.Create;
                using (var fileStream = new System.IO.FileStream(dest_path.ToString(), filemode))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public void Appen(FsFileStatusPage file,string content)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this._fs_rest_client.Append(this._store.GetUri(), file, stream);
            }
        }

        public void Append(FsFileStatusPage file, byte[] bytes)
        {
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this._fs_rest_client.Append(this._store.GetUri(), file, stream);
            }
        }

        public void Concat(IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            this._fs_rest_client.Concat(this._store.GetUri(), src_paths,dest_path);
        }

        public void ClearFileExpiry(FsPath path)
        {
            this._fs_rest_client.SetFileExpiryNever(this._store.GetUri(), path);
        }

        public void SetFileExpiryAbsolute(FsPath path, System.DateTimeOffset expiretime)
        {
            this._fs_rest_client.SetFileExpiry(this._store.GetUri(), path, expiretime);
        }

        public void SetFileExpiryRelativeToNow(FsPath path, System.TimeSpan timespan)
        {
            this._fs_rest_client.SetFileExpiryRelativeToNow(this._store.GetUri(), path, timespan);
        }

        public void SetFileExpiryRelativeToCreationDate(FsPath path, System.TimeSpan timespan)
        {
            this._fs_rest_client.SetFileExpiryRelativeToCreationDate(this._store.GetUri(), path, timespan);
        }

        public ContentSummary GetContentSummary(FsPath path)
        {
            return this._fs_rest_client.GetContentSummary(this._store.GetUri(), path);
        }

        public void SetOwner(FsPath path, string owner, string group)
        {
            this._fs_rest_client.SetOwner(this._store.GetUri(), path, owner, group);
        }        

        public void Move(FsPath src_path, FsPath dest_path)
        {
            this._fs_rest_client.Move(this._store.GetUri(), src_path, dest_path);
        }
    }
}
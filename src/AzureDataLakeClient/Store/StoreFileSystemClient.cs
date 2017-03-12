using System;
using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL = Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store.Models;

namespace AzureDataLakeClient.Store
{
    public class StoreFileSystemClient : AccountClientBase
    {
        private StoreFileSystemRestClient _rest_client;

        StoreUri storeuri;

        public StoreFileSystemClient(StoreUri store, AuthenticatedSession authSession) :
            base(store.Name,authSession)
        {
            _rest_client = new StoreFileSystemRestClient(this.AuthenticatedSession.Credentials);
            this.storeuri = store;
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
            return this._rest_client.ListFilesPaged(this.storeuri, path, options);
        }
        
        public void CreateDirectory(FsPath path)
        {
            this._rest_client.Mkdirs(this.storeuri, path);
        }

        public void Delete(FsPath path)
        {
            _rest_client.Delete(this.storeuri, path);
        }

        public void Delete(FsPath path, bool recursive)
        {
            _rest_client.Delete(this.storeuri, path, recursive );
        }

        public void Create(FsPath path, byte[] bytes, CreateFileOptions options)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            _rest_client.Create(this.storeuri, path,memstream, options);
        }

        public void CreateFileWithContent(FsPath path, string content, CreateFileOptions options)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.Create(path, bytes, options);
        }

        public FsFileStatus GetFileStatus(FsPath path)
        {
            return this._rest_client.GetFileStatus(this.Account, path);
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
            var acl_result = this._rest_client.GetAclStatus(this.storeuri, path);
            return acl_result;
        }

        public void ModifyAclEntries(FsPath path, FsAclEntry entry)
        {
            this._rest_client.ModifyAclEntries(this.storeuri, path, entry);
        }

        public void ModifyAclEntries(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this._rest_client.ModifyAclEntries(this.storeuri, path, entries);
        }

        public void SetAcl(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this._rest_client.SetAcl(this.storeuri, path, entries);
        }

        public void RemoveAcl(FsPath path)
        {
            this._rest_client.RemoveAcl(this.storeuri, path);
        }

        public void RemoveDefaultAcl(FsPath path)
        {
            this._rest_client.RemoveDefaultAcl(this.storeuri, path);
        }

        public System.IO.Stream Open(FsPath path)
        {
            return this._rest_client.Open(this.storeuri, path);
        }

        public System.IO.StreamReader OpenText(FsPath path)
        {
            var s = this._rest_client.Open(this.storeuri, path);
            return new System.IO.StreamReader(s);
        }

        public System.IO.Stream Open(FsPath path, long offset, long bytesToRead)
        {
            return this._rest_client.Open(this.storeuri, path, bytesToRead, offset);
        }

        public void Upload(LocalPath src_path, FsPath dest_path, UploadOptions options)
        {
            var parameters = new ADL.StoreUploader.UploadParameters(src_path.ToString(), dest_path.ToString(), this.Account, isOverwrite: options.Force);
            var frontend = new ADL.StoreUploader.DataLakeStoreFrontEndAdapter(this.Account, this._rest_client._adls_filesys_rest_client);
            var uploader = new ADL.StoreUploader.DataLakeStoreUploader(parameters, frontend);
            uploader.Execute();
        }

        public void Download(FsPath src_path, LocalPath dest_path, DownloadOptions options)
        {
            using (var stream = this._rest_client.Open(this.storeuri, src_path))
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
                this._rest_client.Append(this.storeuri, file, stream);
            }
        }

        public void Append(FsFileStatusPage file, byte[] bytes)
        {
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this._rest_client.Append(this.storeuri, file, stream);
            }
        }

        public void Concat(IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            this._rest_client.Concat(this.storeuri, src_paths,dest_path);
        }

        public void ClearFileExpiry(FsPath path)
        {
            this._rest_client.SetFileExpiry(this.storeuri, path, ExpiryOptionType.NeverExpire, null);
        }

        public void SetFileExpiryAbsolute(FsPath path, System.DateTimeOffset expiretime)
        {
            this._rest_client.SetFileExpiry(this.storeuri, path, ExpiryOptionType.Absolute, expiretime);
        }

        public void SetFileExpiryRelativeToNow(FsPath path, System.TimeSpan timespan)
        {
            this._rest_client.SetFileExpiryRelativeToNow(this.storeuri, path, timespan);
        }

        public void SetFileExpiryRelativeToCreationDate(FsPath path, System.TimeSpan timespan)
        {
            this._rest_client.SetFileExpiryRelativeToCreationDate(this.storeuri, path, timespan);
        }

        public ContentSummary GetContentSummary(FsPath path)
        {
            return this._rest_client.GetContentSummary(this.storeuri, path);
        }

        public void SetOwner(FsPath path, string owner, string group)
        {
            this._rest_client.SetOwner(this.storeuri, path, owner, group);
        }        

        public void Move(FsPath src_path, FsPath dest_path)
        {
            this._rest_client.Move(this.storeuri, src_path, dest_path);
        }
    }
}
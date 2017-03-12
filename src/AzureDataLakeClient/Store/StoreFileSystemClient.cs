using System;
using System.Collections.Generic;
using AzureDataLakeClient.Authentication;
using ADL = Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store.Models;

namespace AzureDataLakeClient.Store
{
    public class StoreFileSystemClient : AccountClientBase
    {
        private StoreFileSystemRestClient _adls_filesys_rest_client;

        public StoreFileSystemClient(string account, AuthenticatedSession authSession) :
            base(account,authSession)
        {
            _adls_filesys_rest_client = new StoreFileSystemRestClient(this.AuthenticatedSession.Credentials);
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
            return this._adls_filesys_rest_client.ListFilesPaged(this.Account, path, options);
        }
        
        public void CreateDirectory(FsPath path)
        {
            this._adls_filesys_rest_client.Mkdirs(this.Account, path);
        }

        public void Delete(FsPath path)
        {
            _adls_filesys_rest_client.Delete(this.Account, path);
        }

        public void Delete(FsPath path, bool recursive)
        {
            _adls_filesys_rest_client.Delete(this.Account, path, recursive );
        }

        public void CreateFileWithContent(FsPath path, byte[] bytes, CreateFileOptions options)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            _adls_filesys_rest_client.Create(this.Account, path,memstream, options);
        }

        public void CreateFileWithContent(FsPath path, string content, CreateFileOptions options)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.CreateFileWithContent(path, bytes, options);
        }

        public FsFileStatus GetFileStatus(FsPath path)
        {
            return this._adls_filesys_rest_client.GetFileStatus(this.Account, path);
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


        public FsAcl GetPermissions(FsPath path)
        {
            var acl_result = this._adls_filesys_rest_client.GetAclStatus(this.Account, path);
            return acl_result;
        }

        public void ModifyACLs(FsPath path, FsAclEntry entry)
        {
            this._adls_filesys_rest_client.ModifyAclEntries(this.Account, path, entry);
        }

        public void ModifyACLs(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this._adls_filesys_rest_client.ModifyAclEntries(this.Account, path, entries);
        }

        public void SetACLs(FsPath path, IEnumerable<FsAclEntry> entries)
        {
            this._adls_filesys_rest_client.SetAcl(this.Account, path, entries);
        }

        public void RemoveAcl(FsPath path)
        {
            this._adls_filesys_rest_client.RemoveAcl(this.Account, path);
        }

        public void RemoveDefaultAcl(FsPath path)
        {
            this._adls_filesys_rest_client.RemoveDefaultAcl(this.Account, path);
        }

        public System.IO.Stream OpenFileForReadBinary(FsPath path)
        {
            return this._adls_filesys_rest_client.Open(this.Account, path);
        }

        public System.IO.StreamReader OpenFileForReadText(FsPath path)
        {
            var s = this._adls_filesys_rest_client.Open(this.Account, path);
            return new System.IO.StreamReader(s);
        }

        public System.IO.Stream OpenFileForReadBinary(FsPath path, long offset, long bytesToRead)
        {
            return this._adls_filesys_rest_client.Open(this.Account, path, bytesToRead, offset);
        }

        public void Upload(LocalPath src_path, FsPath dest_path, UploadOptions options)
        {
            var parameters = new ADL.StoreUploader.UploadParameters(src_path.ToString(), dest_path.ToString(), this.Account, isOverwrite: options.Force);
            var frontend = new ADL.StoreUploader.DataLakeStoreFrontEndAdapter(this.Account, this._adls_filesys_rest_client._adls_filesys_rest_client);
            var uploader = new ADL.StoreUploader.DataLakeStoreUploader(parameters, frontend);
            uploader.Execute();
        }

        public void Download(FsPath src_path, LocalPath dest_path, DownloadOptions options)
        {
            using (var stream = this._adls_filesys_rest_client.Open(this.Account, src_path))
            {
                var filemode = options.Append ? System.IO.FileMode.Append : System.IO.FileMode.Create;
                using (var fileStream = new System.IO.FileStream(dest_path.ToString(), filemode))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public void AppendString(FsFileStatusPage file,string content)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this._adls_filesys_rest_client.Append(this.Account, file, stream);
            }
        }

        public void AppendBytes(FsFileStatusPage file, byte[] bytes)
        {
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this._adls_filesys_rest_client.Append(this.Account, file, stream);
            }
        }

        public void Concatenate(IEnumerable<FsPath> src_paths, FsPath dest_path)
        {
            this._adls_filesys_rest_client.ConcatConcat(this.Account, src_paths,dest_path);
        }

        public void ClearFileExpiry(FsPath path)
        {
            this._adls_filesys_rest_client.SetFileExpiry(this.Account, path, ExpiryOptionType.NeverExpire, null);
        }

        public void SetFileExpiryAbsolute(FsPath path, System.DateTimeOffset expiretime)
        {
            this._adls_filesys_rest_client.SetFileExpiry(this.Account, path, ExpiryOptionType.Absolute, expiretime);
        }

        public void SetFileExpiryRelativeToNow(FsPath path, System.TimeSpan timespan)
        {
            this._adls_filesys_rest_client.SetFileExpiryRelativeToNow(this.Account, path, timespan);
        }

        public void SetFileExpiryRelativeToCreationDate(FsPath path, System.TimeSpan timespan)
        {
            this._adls_filesys_rest_client.SetFileExpiryRelativeToCreationDate(this.Account, path, timespan);
        }

        public ContentSummary GetContentSummary(FsPath path)
        {
            return this._adls_filesys_rest_client.GetContentSummary(this.Account, path);
        }

        public void SetOwner(FsPath path, string owner, string group)
        {
            this._adls_filesys_rest_client.SetOwner(this.Account, path, owner, group);
        }        

        public void Move(FsPath src_path, FsPath dest_path)
        {
            this._adls_filesys_rest_client.Move(this.Account, src_path, dest_path);
        }
    }
}
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.IAppServices
{
    public interface IFileServices
    {
        Task<string> UploadFileAsync(string Id, IFormFile file);
        Task<bool> DeleteFileAsync(string Id);
        Task<string> GetFileUrlAsync(string Id);
    }
}

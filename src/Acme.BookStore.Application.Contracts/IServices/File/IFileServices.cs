using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.IServices.File
{
    public interface IFileServices : ITransientDependency
    {
        Task<bool> UploadFileAsync(string Id, IFormFile file);
        Task<bool> DeleteFileAsync(string Id);
        Task<string> GetFileUrlAsync(string Id);
    }
}

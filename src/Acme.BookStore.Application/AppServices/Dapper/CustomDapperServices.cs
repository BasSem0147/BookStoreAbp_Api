using Acme.BookStore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.AppServices.Dapper
{
    public class CustomDapperServices : ApplicationService
    {
        private readonly ICustomDapperRepository _customRepo;
        public CustomDapperServices(ICustomDapperRepository customRepo)
        {
            _customRepo = customRepo;
        }
        public async Task<string> GetTopAuthorNameAsync()
        {
            return await _customRepo.GetActiveItemAsync();
        }
    }
}

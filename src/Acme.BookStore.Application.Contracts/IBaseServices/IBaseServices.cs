using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.IBaseServices
{
    public interface IBaseServices<DTO, Create_Update> : IApplicationService
    {
        Task<DTO> Update(Create_Update input);
        Task<DTO> Insert(Create_Update input);
        Task<DTO> GetByIdGuid(Guid id);

        Task<PagedResultDto<DTO>> GetAll(GetBaseList input);

        Task<bool> DeleteByIdGuid(Guid id);
    }
    public class GetBaseList : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
    }
}

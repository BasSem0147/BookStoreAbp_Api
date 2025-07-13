using Acme.BookStore.IBaseServices;
using Acme.BookStore.IServices.Author;
using Acme.BookStore.IServices.File;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;


namespace Acme.BookStore.AppServices.Author
{
    public class AuthorAppServices : Acme.BookStore.IAppServices.IAppServices, IAuthorServices
    {
        private readonly IRepository<Acme.BookStore.Models.Author, Guid> _authorRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public IDistributedCache<List<AuthorDto>> _Cache { get; }
        private readonly IFileServices _fileServices;
        public AuthorAppServices(IRepository<Acme.BookStore.Models.Author, Guid> authorRepository, IDistributedCache<List<AuthorDto>> cache,
            IBackgroundJobManager backgroundJobManager, IFileServices fileServices
            )
        {
            _authorRepository = authorRepository;
            _Cache = cache;
            _backgroundJobManager = backgroundJobManager;
            _fileServices = fileServices;
        }


        [Authorize(BookStorePermissions.DeleteAuthorPermission)]
        public async Task<bool> DeleteByIdGuid(Guid id)
        {
            var Author = await _authorRepository.GetAsync(id);
            if (Author == null)
            {
                return false;
            }
            await _authorRepository.DeleteAsync(id);
            await _fileServices.DeleteFileAsync(Path.Combine("Authors", id.ToString()));
            return true;
        }

        public async Task<PagedResultDto<AuthorDto>> GetAll(GetBaseList input)
        {
            //await _backgroundJobManager.EnqueueAsync(
            //new EmailSendingArgs
            //{
            //    EmailAddress = "swebasem@gmail.com",
            //    Subject = "You've successfully registered!",
            //    Body = "..."
            //});
            var authors = await _authorRepository.GetQueryableAsync();

            // ✅ Apply filtering and paging
            var query = authors.AsQueryable()
                .WhereIf(!string.IsNullOrEmpty(input.Filter),
                    x => x.Name.Contains(input.Filter) || x.Surname.Contains(input.Filter));

            var totalCount = query.Count();

            var items = query
                .OrderBy(x => x.Name)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<AuthorDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Acme.BookStore.Models.Author>, List<AuthorDto>>(items)
            };
        }

        public async Task<AuthorDto> GetByIdGuid(Guid id)
        {
            var result = await _authorRepository.GetAsync(id);
            if (result == null)
                return new AuthorDto();
            else
            {
                var imageAttachments = await _fileServices.GetFileUrlAsync(Path.Combine("Authors"+id.ToString()));
                if (!string.IsNullOrEmpty(imageAttachments))
                {
                    result.Picture = imageAttachments.ToString();
                }
            }
                return ObjectMapper.Map<Acme.BookStore.Models.Author, AuthorDto>(result);
        }

        public async Task<AuthorDto> Insert(Create_Update_Author input)
        {
            var validate = new FluentAuthorValidator().Validate(input);
            if (!validate.IsValid)
            {
                throw GetValidationException(validate);
            }
            var mapped = ObjectMapper.Map<Create_Update_Author, Acme.BookStore.Models.Author>(input);
            var inserted = await _authorRepository.InsertAsync(mapped);
            //if (input.Picture != null)
            //    _fileServices.UploadFileAsync(Path.Combine("Authors", inserted.Id.ToString(), "Images"), input.Picture);
            return ObjectMapper.Map<Acme.BookStore.Models.Author, AuthorDto>(inserted);
        }

        public async Task<AuthorDto> Update(Create_Update_Author input)
        {
            var validate = new FluentAuthorValidator().Validate(input);
            if (!validate.IsValid)
            {
                throw GetValidationException(validate);
            }
            var author = await _authorRepository.GetAsync(input.Id);
            var mapped = ObjectMapper.Map(input, author);
            var inserted = await _authorRepository.UpdateAsync(mapped);
            return ObjectMapper.Map<Acme.BookStore.Models.Author, AuthorDto>(inserted);
        }
        public async Task<List<AuthorDto>> GetAllAutorFromCache()
        {
            return await _Cache.GetOrAddAsync("AuthorList", async () =>
            {
                var authors = await GetAllAutorFromDB();
                return authors;
            });
        }
        private async Task<List<AuthorDto>> GetAllAutorFromDB()
        {
            var query = await _authorRepository.GetQueryableAsync();
            var items = await query.Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name+" "+a.Surname,
            }).ToListAsync();
            return items;
        }
    }
}

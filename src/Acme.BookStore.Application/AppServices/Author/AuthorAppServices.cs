using Acme.BookStore.IBaseServices;
using Acme.BookStore.IServices.Author;
using Acme.BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Authorization;
using Acme.BookStore.Permissions;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.BackgroundJobs;
using Acme.BookStore.AppServices.Hangfire;
using System.Net.Mail;
using Acme.BookStore.IAppServices;
using System.IO;
using Microsoft.AspNetCore.Http;


namespace Acme.BookStore.AppServices.Author
{
    public class AuthorAppServices : Acme.BookStore.IAppServices.IAppServices, IAuthorServices
    {
        private readonly IRepository<Acme.BookStore.Models.Author, Guid> _authorRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public IDistributedCache<List<AuthorDto>> _Cache { get; }
        public AuthorAppServices(IRepository<Acme.BookStore.Models.Author, Guid> authorRepository,IDistributedCache<List<AuthorDto>> cache,
            IBackgroundJobManager backgroundJobManager
            )
        {
            _authorRepository = authorRepository;
            _Cache = cache;
            _backgroundJobManager = backgroundJobManager;
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
            var authors = await _authorRepository.WithDetailsAsync(p => p.Books);

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
            //_fileServices.UploadFileAsync(Path.Combine("Authors", inserted.Id.ToString(), "images"), new IFormFile { });
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
            var items = await _authorRepository.GetListAsync();
            return ObjectMapper.Map<List<Acme.BookStore.Models.Author>, List<AuthorDto>>(items);
        }
    }
}

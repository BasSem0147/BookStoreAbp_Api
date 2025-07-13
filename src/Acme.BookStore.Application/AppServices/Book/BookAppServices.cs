using Acme.BookStore.IBaseServices;
using Acme.BookStore.IServices.Book;
using Acme.BookStore.IServices.File;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
namespace Acme.BookStore.AppServices.Book
{
    public class BookAppServices : Acme.BookStore.IAppServices.IAppServices, IBookServices
    {
        private readonly IRepository<Acme.BookStore.Models.Book, Guid> _bookRepository;
        private readonly IFileServices _fileServices;

        public BookAppServices(IRepository<Acme.BookStore.Models.Book, Guid> bookRepository, IFileServices fileServices)
        {
            _bookRepository = bookRepository;
            _fileServices = fileServices;
        }
        public async Task<bool> DeleteByIdGuid(Guid id)
        {
            var book =await _bookRepository.GetAsync(id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }
            await _bookRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PagedResultDto<BookDto>> GetAll(GetBaseList input)
        {
            var query = await _bookRepository.WithDetailsAsync(a=>a.Author);

            if (!string.IsNullOrEmpty(input.Filter))
            {
                query = query.Where(x => x.Name.Contains(input.Filter));
            }
            // Use Dynamic LINQ for sorting
            query = query.OrderBy(input.Sorting ?? "Name");
            var totalCount = await query.CountAsync();
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var books =await query.Select(x => new BookDto
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                PublishDate = x.PublishDate,
                Price = x.Price,
                AuthorId = x.AuthorId,
                AuthorName = x.Author.Name
            }).ToListAsync();
            foreach (var item in books)
            {
                var imageAttachments = await _fileServices.GetFileUrlAsync(Path.Combine("Books" + item.Id.ToString()));
                if (!string.IsNullOrEmpty(imageAttachments))
                {
                    item.Picture = imageAttachments.ToString();
                }
            }
            return new PagedResultDto<BookDto>
            {
                TotalCount = totalCount,
                Items = books
            };
        }

        public async Task<BookDto> GetByIdGuid(Guid id)
        {
            var query = await _bookRepository.WithDetailsAsync(x => x.Author);
            var book = query.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }
            return new BookDto
            {
                Id = book.Id,
                Name = book.Name,
                Type = book.Type,
                PublishDate = book.PublishDate,
                Price = book.Price,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.Name
            };
        }

        public async Task<BookDto> Insert(Create_Update_Book input)
        {
           var validate=new FluentBookValidator().Validate(input);
            if (!validate.IsValid)
            {
                throw GetValidationException(validate);
            }
            var mapped = ObjectMapper.Map<Create_Update_Book, Acme.BookStore.Models.Book>(input);
            var inserted = await _bookRepository.InsertAsync(mapped);
            return ObjectMapper.Map<Acme.BookStore.Models.Book, BookDto>(inserted);
        }

        public async Task<BookDto> Update(Create_Update_Book input)
        {
           var book = await _bookRepository.GetAsync(input.Id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }
            var validate = new FluentBookValidator().Validate(input);
            if (!validate.IsValid)
            {
                throw GetValidationException(validate);
            }
            var mapped = ObjectMapper.Map<Create_Update_Book, Acme.BookStore.Models.Book>(input);
            var updated =await _bookRepository.UpdateAsync(mapped);
            return ObjectMapper.Map<Acme.BookStore.Models.Book, BookDto>(updated);
        }
    }
}

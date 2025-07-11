using Acme.BookStore.IServices.Author;
using Acme.BookStore.Models;
using AutoMapper;

namespace Acme.BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        CreateMap<Author,AuthorDto>().ReverseMap();
        CreateMap<Author, Create_Update_Author>().ReverseMap();
    }
}

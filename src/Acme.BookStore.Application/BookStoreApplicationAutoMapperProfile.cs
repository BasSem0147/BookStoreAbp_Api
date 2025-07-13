using Acme.BookStore.IServices.Author;
using Acme.BookStore.IServices.Book;
using Acme.BookStore.Models;
using AutoMapper;

namespace Acme.BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        CreateMap<Author,AuthorDto>().ReverseMap();
        CreateMap<Author, Create_Update_Author>().ReverseMap();

        CreateMap<Book, Create_Update_Book>()
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
            .ReverseMap()
            .ForMember(dest => dest.Author, opt => opt.Ignore());
        CreateMap<Book, BookDto>().ReverseMap();
    }
}

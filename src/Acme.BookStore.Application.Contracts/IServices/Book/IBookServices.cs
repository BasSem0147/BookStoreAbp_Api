using Acme.BookStore.IBaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.IServices.Book
{
    public interface IBookServices:IBaseServices<BookDto, Create_Update_Book>
    {
    }
}

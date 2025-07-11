using Acme.BookStore.IBaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.IServices.Author
{
    public interface IAuthorServices:IBaseServices<AuthorDto,Create_Update_Author>
    {
    }
}

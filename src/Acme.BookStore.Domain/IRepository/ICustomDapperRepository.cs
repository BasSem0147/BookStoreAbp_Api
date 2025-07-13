using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.IRepository
{
    public interface ICustomDapperRepository
    {
        Task<string> GetActiveItemAsync();
    }
}

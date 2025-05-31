using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;

namespace Data.Interfaces
{
    public interface IBookData : IBaseModelData<Book>
    {
        Task<bool> UpdatePartial(Book book);
    }
}

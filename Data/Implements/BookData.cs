using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Model;

namespace Data.Implements
{
    public class BookData : BaseModelData<Book>, IBookData
    {

        public BookData(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<bool> UpdatePartial(Book book)
        {
            var existingBook = await _dbSet.FindAsync(book.Id);
            if (existingBook == null) return false;
            // Actualizar solo los campos que se deseen modificar
            existingBook.Name = book.Name;
            existingBook.Description = book.Description;
            existingBook.AuthorId = book.AuthorId;
            _context.Set<Book>().Update(existingBook);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

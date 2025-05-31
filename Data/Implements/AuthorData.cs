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
    public class AuthorData : BaseModelData<Author>, IAuthorData
    {
        public AuthorData(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<bool> UpdatePartial(Author author)
        {
            var existingAuthor = await _dbSet.FindAsync(author.Id);
            if (existingAuthor == null) return false;
            // Actualizar solo los campos que se deseen modificar
            existingAuthor.Name = author.Name;
            existingAuthor.Description = author.Description;
            _context.Set<Author>().Update(existingAuthor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

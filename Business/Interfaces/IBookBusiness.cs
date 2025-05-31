using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.AuthorDto;
using Entity.Dtos.BookDto;
using Entity.Dtos.CityDto;
using Entity.Dtos.ClientDto;
using Entity.Model;

namespace Business.Interfaces
{
    public interface IBookBusiness : IBaseBusiness<Book, BookDto>
    {
        /// <summary>
        /// Actualiza parcialmente los datos de un book.
        /// </summary>
        /// <param name="dto">Objeto que contiene los datos actualizados del book, como nombre o estado.</param>
        ///<returns>True si la actualización fue exitosa; de lo contario false</returns>
        Task<bool> UpdatePartialBookAsync(BookUpdateDto dto);

    }
}

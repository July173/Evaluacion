using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.AuthorDto;
using Entity.Dtos.ClientDto;
using Entity.Model;

namespace Business.Interfaces
{
    public interface IAuthorBusiness : IBaseBusiness<Author, AuthorDto>
    {
        /// <summary>
        /// Actualiza parcialmente los datos de un autor.
        /// </summary>
        /// <param name="dto">Objeto que contiene los datos actualizados del autor, como nombre o estado.</param>
        ///<returns>True si la actualización fue exitosa; de lo contario false</returns>
        Task<bool> UpdatePartialAuthotAsync(AuthorUpdateDto dto);

    }
}

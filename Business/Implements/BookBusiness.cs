using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.AuthorDto;
using Entity.Dtos.BookDto;
using Entity.Dtos.CityDto;
using Entity.Model;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public  class BookBusiness : BaseBusiness<Book,BookDto>, IBookBusiness
    {
        private readonly IBookData _bookData;
        public BookBusiness(IBookData bookData, IMapper mapper, ILogger<BookBusiness> logger ) :base( bookData, mapper, logger)
        {
            _bookData = bookData;
        }
     
      

        ///<summary>
        /// Actualiza parcialmente un libro en la base de datos
        /// </summary>
        public async Task<bool> UpdatePartialBookAsync(BookUpdateDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");


            var book = _mapper.Map<Book>(dto);

            var result = await _bookData.UpdatePartial(book); // esto ya retorna bool
            return result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entity.Dtos.CityDto;
using Entity.Model;

namespace Utilities.Mappers.Profiles
{
    public class BookProfile :Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, BookUpdateDto>().ReverseMap();
        }
    }
}

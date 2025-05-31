using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entity.Dtos.AuthorDto;
using Entity.Dtos.ClientDto;
using Entity.Model;

namespace Utilities.Mappers.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Author, AuthorUpdateDto>().ReverseMap();
        }
    }
}

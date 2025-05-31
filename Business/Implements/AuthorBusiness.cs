using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.AuthorDto;
using Entity.Dtos.ClientDto;
using Entity.Model;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class AuthorBusiness : BaseBusiness<Author, AuthorDto>, IAuthorBusiness
    {
        private readonly IAuthorData _authorData;
        private readonly IValidator<AuthorDto> _validator;

        public AuthorBusiness(IAuthorData authorData, IMapper mapper, ILogger<AuthorBusiness> logger)
            : base(authorData, mapper, logger)
        {
            _authorData = authorData;
         
        }

        public async Task<bool> UpdatePartialAuthorAsync(AuthorUpdateDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");
            var author = _mapper.Map<Author>(dto);
            var result = await _authorData.UpdatePartial(author);
            return result;
        }
    }
}

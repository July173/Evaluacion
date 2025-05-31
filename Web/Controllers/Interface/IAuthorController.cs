using Microsoft.AspNetCore.Mvc;
using Entity.Model;
using Entity.Dtos.Base;
using Entity.Dtos.ClientDto;
using Entity.Dtos.AuthorDto;


namespace Web.Controllers.Interface
{
    public interface IAuthorController : IGenericController<AuthorDto, Author>
    {
        Task<IActionResult> UpdatePartial(int id,  AuthorUpdateDto dto);
    }
}



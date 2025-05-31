using Microsoft.AspNetCore.Mvc;
using Entity.Model;
using Entity.Dtos.Base;
using Entity.Dtos.ClientDto;


namespace Web.Controllers.Interface
{
    public interface IClientController : IGenericController<AuthorDto, Client>
    {
        Task<IActionResult> UpdatePartial(int id, int clientId, AuthorActiveDto dto);
        Task<IActionResult> DeleteLogic(int id);
    }
}



using Business.Interfaces;
using Entity.Dtos.AuthorDto;
using Entity.Dtos.Base;
using Entity.Dtos.ClientDto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Interface;

namespace Web.Controllers.Implements;

[Route("api/[controller]")]
public class AuthorController : GenericController<AuthorDto, Author>, IAuthorController
{
    private readonly IAuthorBusiness _authorBusiness;
    public AuthorController(IAuthorBusiness authorBusiness, ILogger<AuthorController> logger)
        : base(authorBusiness, logger)
    {
        _authorBusiness = authorBusiness;
    }

    protected override int GetEntityId(AuthorDto dto)
    {
        return dto.Id;
    }

    [HttpPatch]

    public async Task<IActionResult> UpdatePartial(int id, [FromBody] AuthorUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authorBusiness.UpdatePartialAuthorAsync(dto);
            return Ok(new { Success = result });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error de validación al actualizar parcialmente el autor: {ex.Message}");
            return BadRequest(ex.Message);  
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al actualizar parcialmente el autor : {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

   
}
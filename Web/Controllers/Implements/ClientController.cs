using Business.Interfaces;
using Entity.Dtos.Base;
using Entity.Dtos.ClientDto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Interface;

namespace Web.Controllers.Implements;

[Route("api/[controller]")]
public class ClientController : GenericController<AuthorDto, Client>, IClientController
{
    private readonly IClientBusiness _clientBusiness;
    public ClientController(IClientBusiness clientBusiness, ILogger<ClientController> logger)
        : base(clientBusiness, logger)
    {
        _clientBusiness = clientBusiness;
    }

    protected override int GetEntityId(AuthorDto dto)
    {
        return dto.Id;
    }

    [HttpPatch]

    public async Task<IActionResult> UpdatePartial(int id, int clientId, [FromBody] AuthorActiveDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _clientBusiness.UpdatePartialAsync(dto);
            return Ok(new { Success = result });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error de validación al actualizar parcialmente el cliente: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al actualizar parcialmente el cliente: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("logic/{id}")]

    public async Task<IActionResult> DeleteLogic(int id)
    {
        try
        {
            var dto = new GenericDto { Id = id, Active = false }; // Se inicializa la propiedad requerida 'Status'
            var result = await _clientBusiness.ActiveAsync(dto);
            if (!result)
                return NotFound($"Cliente con ID {id} no encontrado");
            return Ok(new { Success = true });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error de validación al eliminar lógicamente cliente: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al eliminar lógicamente el cliente con ID {id}: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
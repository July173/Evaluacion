using Business.Interfaces;
using Entity.Dtos.Base;
using Entity.Dtos.CityDto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Interface;
namespace Web.Controllers.Implements;

[Route("api/[controller]")]

public class CityController : GenericController<BookDto, Author>, ICityController
{
    private readonly ICityBusiness _cityBusiness;
    public CityController(ICityBusiness cityBusiness, ILogger<CityController> logger)
        : base(cityBusiness, logger)
    {
        _cityBusiness = cityBusiness;
    }

    protected override int GetEntityId(BookDto dto)
    {
        return dto.Id;
    }

    [HttpPatch]

    public async Task<IActionResult> UpdatePartial(int id, int cityId, [FromBody] BookUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _cityBusiness.UpdatePartialAsync(dto);
            return Ok(new { Success = result });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error de validación al actualizar parcialmente la ciudad: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al actualizar parcialmente la ciudad: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("logic/{id}")]

    public async Task<IActionResult> DeleteLogic(int id)
    {
        try
        {
            var dto = new GenericDto { Id = id, Active = false }; // Se inicializa la propiedad requerida 'Status'
            var result = await _cityBusiness.ActiveAsync(dto);
            if (!result)
                return NotFound($"Ciudad con ID {id} no encontrado");
            return Ok(new { Success = true });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error de validación al eliminar lógicamente ciudad: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al eliminar lógicamente la ciudad con ID {id}: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
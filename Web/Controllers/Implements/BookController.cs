using Business.Interfaces;
using Entity.Dtos.BookDto;
using Entity.Dtos.Base;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Interface;
using Entity.Dtos.CityDto;

namespace Web.Controllers.Implements;

[Route("api/[controller]")]
public class BookController : GenericController<BookDto, Book>, IBookController
{
    private readonly IBookBusiness _bookBusiness;
    public BookController(IBookBusiness bookBusiness, ILogger<BookController> logger)
        : base(bookBusiness, logger)
    {
        _bookBusiness = bookBusiness;
    }

    protected override int GetEntityId(BookDto dto) 
    {
        return dto.Id;
    }

    [HttpPatch]
    public async Task<IActionResult> UpdatePartial(int id, [FromBody] BookUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bookBusiness.UpdatePartialBookAsync(dto);
            return Ok(new { Success = result });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error de validación al actualizar parcialmente el libro: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al actualizar parcialmente el libro: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}

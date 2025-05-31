using Microsoft.AspNetCore.Mvc;
using Entity.Model;
using Entity.Dtos.Base;
using Entity.Dtos.CityDto;

namespace Web.Controllers.Interface
{
    public interface ICityController : IGenericController<BookDto, Author>
    {
        Task<IActionResult> UpdatePartial(int id, int cityId, BookUpdateDto dto);
        Task<IActionResult> DeleteLogic(int id);
    }
}



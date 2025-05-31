using Microsoft.AspNetCore.Mvc;
using Entity.Model;
using Entity.Dtos.Base;
using Entity.Dtos.CityDto;

namespace Web.Controllers.Interface
{
    public interface IBookController : IGenericController<BookDto, Book>
    {
        Task<IActionResult> UpdatePartial(int id,  BookUpdateDto dto);
    }
}



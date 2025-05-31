using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.Base;
using Entity.Model;

namespace Entity.Dtos.CityDto
{
    public class BookUpdateDto : BaseDto
    {
        public int AuthorId { get; set; }

    }
}

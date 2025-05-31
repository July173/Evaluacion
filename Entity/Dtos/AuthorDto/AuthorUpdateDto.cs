using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.Base;

namespace Entity.Dtos.AuthorDto
{
    public class AuthorUpdateDto :BaseDto
    {
        public string Nacionality { get; set; }
        
    }
}

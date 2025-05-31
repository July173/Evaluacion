using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.Base;
using Entity.Model;

namespace Entity.Dtos.ClientDto
{
    public class AuthorActiveDto 
    {
        public int Id { get; set; }
        public bool Active { get; set; }
    } 
}

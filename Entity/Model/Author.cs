using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Base;

namespace Entity.Model
{
    public class Author : BaseModel
    {
        public string Nacionality { get; set; }
        public ICollection<Book> Books { get; set; }

    }
}

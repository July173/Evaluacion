using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Base;

namespace Entity.Model
{
    public class Book : BaseModel
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}

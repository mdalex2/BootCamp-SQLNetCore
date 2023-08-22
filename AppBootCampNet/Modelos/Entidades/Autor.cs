using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Autor
    {
        [Key]
        [Column("AutorId")]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string WebUrl { get; set; }
        public List<AutorLibro> AutorLibros { get; set; }
    }
}

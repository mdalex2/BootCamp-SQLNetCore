using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class CategoriaLibro
    {
        [Key]
        [Column("CategoriaLibroId")]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }
    }
}

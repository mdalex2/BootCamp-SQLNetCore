using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Libro
    {
        [Key]
        [Column("LibroId")]
        public Int64 Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPublicado { get; set; }
        public string ImagenUrl { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
        public int CategoriaLibroId { get; set; }
        
        [ForeignKey("CategoriaLibroId")]
        public CategoriaLibro CategoriaLibro { get; set; }
        public List<AutorLibro> LibrosAutores { get; set; }
        public PrecioOferta PrecioOferta { get; set; }
        public List<Reviews> Reviews { get; set; }
    }
}

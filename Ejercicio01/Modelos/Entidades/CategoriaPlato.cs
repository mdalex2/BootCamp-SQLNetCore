using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    [Table("CategoriasPlato")]
    public class CategoriaPlato
    {
        [Column("CategoriaPlatoId")]
        [Key]
        public int Id { get; set; }
        public string Categoria { get; set; }
        public string Icono { get; set; }
        public bool Activo { get; set; }
    }
}

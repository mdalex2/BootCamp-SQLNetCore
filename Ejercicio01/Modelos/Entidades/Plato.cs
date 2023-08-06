using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    [Table("Platos")]
    public class Plato
    {
        [Key]
        [Column("PlatoId")]
        public Int64 Id { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public string DescripcionPlato { get; set; }
        public string Foto { get; set; }
        public bool Activo { get; set; }

        public int CategoriaPlatoId { get; set; }
        [ForeignKey("CategoriaPlatoId")]
        public CategoriaPlato CategoriaPlato { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class PrecioOferta
    {
        [Column("PrecioOfertaId")]
        public long Id { get; set; }
        public decimal NuevoPrecio { get; set; }
        public string TextoPromocional { get; set; }
        public long LibroId { get; set; }

        [ForeignKey("LibroId")]
        public Libro Libro { get; set; }
    }
}

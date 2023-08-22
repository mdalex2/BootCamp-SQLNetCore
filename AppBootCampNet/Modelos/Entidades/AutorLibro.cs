using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class AutorLibro
    {
        [Key]
        [Column("AutorLibroId")]
        public long Id { get; set; }
        public int AutorId { get; set; }
        public long LibroId { get; set; }
        public long Orden { get; set; }

        [ForeignKey("AutorId")]
        public Autor Autor { get; set; }

        [ForeignKey("LibroId")]
        public Libro Libro { get; set; }
    }
}

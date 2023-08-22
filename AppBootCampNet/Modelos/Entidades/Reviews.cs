using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Reviews
    {
        [Key]
        [Column("ReviewId")]
        public long Id { get; set; }
        public string NombreVotante { get; set; }
        public int NroEstrellas { get; set; }
        public string Comentario { get; set; }
        public long LibroId { get; set; }

        [ForeignKey("LibroId")]
        public Libro Libro { get; set; }
    }
}

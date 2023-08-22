using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades.Dtos
{
    public class LibroDetalleDto
    {
        public Int64 LibroId { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public DateTime FechaPublicado { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioActual { get; set; }
        public string TextoPromocional { get; set; }
        public string Autores { get; set; }
        public int ReviewCantidad { get; set; }
        public double? ReviewPromedio { get; set; }

    }
}

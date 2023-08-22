using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    //[Keyless]
    public class LibroRango
    {
        public string NombreAutor { get; set; }
        public string TituloLibro { get; set; }
        public string TipoLibro { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}

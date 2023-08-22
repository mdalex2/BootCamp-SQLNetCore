using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using Modelos.Entidades.Dtos;
using Modelos.Especificaciones;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LINQController : ControllerBase
    {
        private readonly AppDBContext _db;
        public LINQController(AppDBContext db)
        {
            _db = db;
        }



        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibrosQuery()
        {

            var lista = await ( from l in _db.Libro.Include( i => i.CategoriaLibro )
                                  select l ).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLLibrosMethod()
        {
            var lista = await _db.Libro.Include( i => i.CategoriaLibro ).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibrosPorNombreQuery(string titulo)
        {

            var lista = await (from l in _db.Libro.Include(i => i.CategoriaLibro)
                               where l.Titulo ==  titulo
                               select l).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibrosContainsNombreQuery(string titulo)
        {

            var lista = await (from l in _db.Libro.Include(i => i.CategoriaLibro)
                               where l.Titulo.Contains(titulo)
                               orderby l.Titulo
                               select l).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibrosLikeStartNombreQuery(string titulo)
        {

            var lista = await (from l in _db.Libro.Include(i => i.CategoriaLibro)
                               where l.Titulo.StartsWith(titulo)
                               orderby l.Titulo,l.Descripcion
                               select l).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibrosLikeDBFunctNombreQuery(string titulo)
        {
            string filtro = $"%{titulo}";

            var lista = await (from l in _db.Libro.Include(i => i.CategoriaLibro)
                               where EF.Functions.Like(l.Titulo,filtro)
                               orderby l.Titulo,l.CategoriaLibro.Nombre ascending
                               orderby l.FechaPublicado descending
                               select l).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibrosLikeProyectarNombreQuery(string titulo)
        {

            var lista = await (from l in _db.Libro.Include(i => i.CategoriaLibro)
                               where l.Titulo.Contains(titulo)
                               orderby l.Titulo, l.Descripcion
                               select new { l.Titulo,l.Precio,l.CategoriaLibro.Nombre}).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibrosLikeProyectarDTONombreQuery(string titulo)
        {

            var lista = await (from l in _db.Libro.Include(i => i.CategoriaLibro)
                               where l.Titulo.Contains(titulo)
                               orderby l.Titulo, l.Descripcion
                               select new LibroDto { Titulo = l.Titulo, Precio = l.Precio, Categoria = l.CategoriaLibro.Nombre }).ToListAsync();
            return Ok(lista);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibrosLikeProyectarDTONombreMethod(string titulo)
        {

            var lista = await _db.Libro.Include(i => i.CategoriaLibro)
                               .Where(l => l.Titulo.Contains(titulo))
                               .OrderBy(l => l.Titulo).OrderBy(l => l.Descripcion)
                               .Select(l => 
                                    new LibroDto { 
                                        Titulo = l.Titulo, Precio = l.Precio, Categoria = l.CategoriaLibro.Nombre 
                                    }).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetCategoriaLikeProyectarQuery(string titulo)
        {

            var lista = await (from l in _db.CategoriaLibro.Include(i => i.Libros)
                               where l.Nombre.Contains(titulo)
                               orderby l.Nombre
                               select new {
                                   Id = l.Id,
                                   Nombre = l.Nombre,
                                   Libros = l.Libros.Select(s => new {
                                                        Titulo = s.Titulo, 
                                                        Precio = s.Precio 
                                            })
                               }).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosUnionQuery()
        {
            var lista1 = await (from l in _db.Libro.Include(l => l.CategoriaLibro)
                                orderby l.Titulo
                                where l.Precio >= 40
                                select new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.CategoriaLibro.Nombre,
                                    Precio = l.Precio,
                                    FechaPublicado = l.FechaPublicado
                                }
                                ).ToListAsync();
            //union no registra lso duplicados
            //concat permite duplicados al unir
            lista1.Union ( await (from l in _db.Libro.Include(l => l.CategoriaLibro)
                                orderby l.Titulo
                                where l.FechaPublicado.Year >= 2013
                                select new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.CategoriaLibro.Nombre,
                                    Precio = l.Precio,
                                    FechaPublicado = l.FechaPublicado
                                }
                                ).ToListAsync());
            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosUnionMethod()
        {
            var lista1 = await _db.Libro.Include(l => l.CategoriaLibro)
                                .OrderBy( l => l.Titulo)
                                .Where( l => l.Precio >= 40)
                                .Select( l => new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.CategoriaLibro.Nombre,
                                    Precio = l.Precio,
                                    FechaPublicado = l.FechaPublicado
                                }).ToListAsync();
            //union no registra los duplicados
            //concat permite duplicados al unir
            lista1.Union(await _db.Libro.Include(l => l.CategoriaLibro)
                                .OrderBy(l => l.Titulo)
                                .Where(l => l.FechaPublicado.Year >= 2000)
                                .Select( l => new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.CategoriaLibro.Nombre,
                                    Precio = l.Precio,
                                    FechaPublicado = l.FechaPublicado
                                }).ToListAsync());
            return Ok(lista1);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosUnionAll_ConcatQuery()
        {
            var lista1 = await (from l in _db.Libro.Include(l => l.CategoriaLibro)
                                orderby l.Titulo
                                where l.Precio >= 40
                                select new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.CategoriaLibro.Nombre,
                                    Precio = l.Precio,
                                    FechaPublicado = l.FechaPublicado
                                }
                                ).ToListAsync();
            //union no registra lso duplicados
            //concat permite duplicados al unir = UNION ALL
            lista1.Concat(await (from l in _db.Libro.Include(l => l.CategoriaLibro) //UNION ALL = Concat
                                where l.FechaPublicado.Year >= 2013
                                select new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.CategoriaLibro.Nombre,
                                    Precio = l.Precio,
                                    FechaPublicado = l.FechaPublicado
                                }
                                ).ToListAsync());
            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosUnionAll_ConcatMethod()
        {
            var lista1 = await _db.Libro.Include(l => l.CategoriaLibro)
                                .Where(l => l.Precio >= 40)
                                .Select(l => new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.CategoriaLibro.Nombre,
                                    Precio = l.Precio,
                                    FechaPublicado = l.FechaPublicado
                                }).ToListAsync();
            //union no registra los duplicados
            //concat permite duplicados al unir = UNION ALL
            lista1.Concat(await _db.Libro.Include(l => l.CategoriaLibro)
                                .Where(l => l.FechaPublicado.Year >= 40)
                                .Select(l => new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.CategoriaLibro.Nombre,
                                    Precio = l.Precio,
                                    FechaPublicado = l.FechaPublicado
                                }).ToListAsync());
            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByCountQuery()
        {
            var lista1 = await (from l in _db.Libro.Include(l => l.CategoriaLibro)
                                orderby l.Titulo
                                group l by l.CategoriaLibro.Nombre into grpCateg
                                //where l.Precio >= 40
                                select new
                                {
                                    Categoria = grpCateg.Key,
                                    Cantidad = grpCateg.Count()                                    
                                }
                                ).ToListAsync();
            
            return Ok(lista1);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByCountMethod()
        {
            var lista1 = await _db.Libro.Include(l => l.CategoriaLibro)
                                .OrderBy(l => l.Titulo)
                                .GroupBy(l => l.CategoriaLibro.Nombre)
                                .Select(g => new
                                {
                                    Categoria = g.Key,
                                    Cantidad = g.Count()                                    
                                }).ToListAsync();
            
            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByMiltipleColumnCountQuery()
        {
            var lista1 = await (from l in _db.Libro.Include(l => l.CategoriaLibro)
                                orderby l.Titulo
                                group l by new { l.CategoriaLibro.Nombre, l.Titulo } into grpCateg
                                //where l.Precio >= 40
                                select new
                                {
                                    Categoria = grpCateg.Key,
                                    Cantidad = grpCateg.Count()
                                }
                                ).ToListAsync();

            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByMaxQuery()
        {
            var lista1 = await (from l in _db.Libro.Include(l => l.CategoriaLibro)
                                orderby l.Titulo
                                group l by l.CategoriaLibro.Nombre into grpCateg
                                //where l.Precio >= 40
                                select new
                                {
                                    Categoria = grpCateg.Key,
                                    PrecioMaximo = (from l2 in grpCateg select l2.Precio).Max()
                                }
                                ).ToListAsync();

            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByMaxMethod()
        {
            var lista1 = await _db.Libro.Include(l => l.CategoriaLibro)
                                .OrderBy(l => l.Titulo)
                                .GroupBy(l => l.CategoriaLibro.Nombre)
                                .Select(g => new
                                {
                                    Categoria = g.Key,
                                    PrecioMaximo = g.Select(l2 => l2.Precio).Max()
                                }).ToListAsync();

            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByMinQuery()
        {
            var lista1 = await (from l in _db.Libro.Include(l => l.CategoriaLibro)
                                orderby l.Titulo
                                group l by l.CategoriaLibro.Nombre into grpCateg
                                select new
                                {
                                    Categoria = grpCateg.Key,
                                    PrecioMinimo = (from l2 in grpCateg select l2.Precio).Min()
                                }
                                ).ToListAsync();

            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByMinMethod()
        {
            var lista1 = await _db.Libro.Include(l => l.CategoriaLibro)
                                .OrderBy(l => l.Titulo)
                                .GroupBy(l => l.CategoriaLibro.Nombre)
                                .Select(g => new
                                {
                                    Categoria = g.Key,
                                    PrecioMinimo = g.Select(l2 => l2.Precio).Min()
                                }).ToListAsync();

            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByAVG_PromedioQuery()
        {
            var lista1 = await (from l in _db.Libro.Include(l => l.CategoriaLibro)
                                orderby l.Titulo
                                group l by l.CategoriaLibro.Nombre into grpCateg
                                //where l.Precio >= 40
                                select new
                                {
                                    Categoria = grpCateg.Key,
                                    PrecioPromedio = (from l2 in grpCateg select l2.Precio).Average()
                                }
                                ).ToListAsync();

            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByAVG_PromedioMethod()
        {
            var lista1 = await _db.Libro.Include(l => l.CategoriaLibro)
                                .OrderBy(l => l.Titulo)
                                .GroupBy(l => l.CategoriaLibro.Nombre)
                                .Select(g => new
                                {
                                    Categoria = g.Key,
                                    PrecioPromedio = g.Select(l2 => l2.Precio).Average()
                                }).ToListAsync();

            return Ok(lista1);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDetalleDto>>> GetLibrosDetalle()
        {
            var lista = await _db.Libro.Include(i => i.CategoriaLibro)
                                       .Include(i => i.PrecioOferta)
                                       .Include(i => i.LibrosAutores).ThenInclude(i => i.Autor) //then para acceder a los autores desde libro autor
                                       .Include(i => i.Reviews)
                            .ToListAsync();

            var listaDetalle = lista.Select(s => new LibroDetalleDto
                                    {
                                        LibroId = s.Id,
                                        Titulo = s.Titulo,
                                        Categoria = s.CategoriaLibro.Nombre,
                                        Precio = s.Precio,
                                        FechaPublicado = s.FechaPublicado,
                                        PrecioActual = s.PrecioOferta == null ? s.Precio : s.PrecioOferta.NuevoPrecio,
                                        TextoPromocional = s.PrecioOferta == null ? null : s.PrecioOferta.TextoPromocional,
                                        Autores = string.Join(", ",s.LibrosAutores.OrderBy(o => o.Orden).Select(s => s.Autor.Nombre)),
                                        ReviewCantidad = s.Reviews.Count(),
                                        ReviewPromedio = s.Reviews.Select(s => (double?)s.NroEstrellas).Average()
                                    }).ToList();
            return Ok(listaDetalle);
        }

        [HttpGet]
        public async Task<ActionResult> GetRangoSP(int rangoInicio, int rangoFin)
        {
            var lista = await _db.LibroRango.FromSqlRaw("exec LibrosPublicadosRango {0},{1}", rangoInicio,rangoFin).ToListAsync();
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<LibroRango>>> CambioPrecioCategoriaSP(CambioPrecioParametros p)
        {
            await _db.Database.ExecuteSqlAsync
                ($"CambioPrecioCategoria @categoriaId =  {p.CategoriaId}, @porcentajeDesc = {p.Porcentaje}, @texto = {p.Texto}");
            return Ok("Cambio de precio ejecutado correctamente");            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDto>>> GetLibroHavingQuery()
        {
            var lista = await (from l in _db.Libro.Include(i => i.CategoriaLibro)
                               group l by l.CategoriaLibro.Nombre into grp
                               where grp.Count() > 2
                               select new
                               {
                                   Categoria = grp.Key,
                                   Cantidad = grp.Count()
                               }).ToListAsync();
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDto>>> GetLibroHavingMethod()
        {
            var lista = await _db.Libro.Include(i => i.CategoriaLibro)
                               .GroupBy(g => g.CategoriaLibro.Nombre)
                               .Where(w =>  w.Count() > 2)
                               .Select(s =>  new
                               {
                                   Categoria = s.Key,
                                   Cantidad = s.Count()
                               }).ToListAsync();
            return Ok(lista);
        }
    }
}

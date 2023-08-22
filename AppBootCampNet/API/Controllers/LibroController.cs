using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly AppDBContext _db;
        public LibroController(AppDBContext db)
        {
            _db = db;  
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriaLibro>>> GetCategoriasLibro()
        {
            var lista = await _db.CategoriaLibro.ToListAsync();
            return Ok(lista);
        }
        [HttpGet]
        public async Task<ActionResult<List<CategoriaLibro>>> GetListaLibros()
        {
            var lista = await _db.Libro.Include(c => c.CategoriaLibro).ToListAsync();
            return Ok(lista);
        }

    }
}

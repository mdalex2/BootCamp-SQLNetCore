using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Ejercicio01Controller : ControllerBase
    {
        private readonly AppDBContext _db;
        public Ejercicio01Controller(AppDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriaPlato>>>GetCategorias()
        {
            var lista = await _db.CategoriaPlato.ToListAsync();
            return Ok(lista);
        }
        [HttpGet]
        public async Task<ActionResult<List<Plato>>> GetPlatos()
        {
            var lista = await _db.Plato.Include(c => c.CategoriaPlato).ToListAsync();
            return Ok(lista);
        }
    }
}

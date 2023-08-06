using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CategoriaPlato> CategoriaPlato { get; set; }
        public DbSet<Plato> Plato { get; set; }
    }
}

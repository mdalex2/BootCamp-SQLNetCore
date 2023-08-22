using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CategoriaLibro> CategoriaLibro { get; set; }
        public DbSet<Libro> Libro { get; set; }
        public DbSet<PrecioOferta> PrecioOferta { get; set; }
        public DbSet<Autor> Autor { get; set; }
        public DbSet<AutorLibro> AutorLibro { get; set; }
        public DbSet<LibroRango> LibroRango { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<LibroRango>(e => e.HasNoKey());
        }
    }

}

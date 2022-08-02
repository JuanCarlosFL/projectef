using Microsoft.EntityFrameworkCore;
using projectef.Models;

namespace projectef;

public class TareasContext : DbContext
{
    public TareasContext(DbContextOptions<TareasContext> options) : base(options)
    {
    }

    public DbSet<Tarea> Tareas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        List<Categoria> categoriasInit = new();
        categoriasInit.Add(new Categoria { CategoriaId = Guid.Parse("39275625-39fc-4f59-b678-0d197827370c"), Nombre = "Actividades pendientes", Descripcion = "Tareas personales", Peso = 20 });
        categoriasInit.Add(new Categoria { CategoriaId = Guid.Parse("b5ec5d03-49ac-4835-accd-0b5da0d1e7c9"), Nombre = "Actividades personales", Descripcion = "Tareas personales", Peso = 50 });

        modelBuilder.Entity<Categoria>(categoria =>{
            categoria.ToTable("Categoria");
            categoria.HasKey(p => p.CategoriaId);
            categoria.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            categoria.Property(p => p.Descripcion).IsRequired(false);
            categoria.Property(p => p.Peso);

            categoria.HasData(categoriasInit);
        });

        List<Tarea> tareasInit = new();
        tareasInit.Add(new Tarea { TareaId = Guid.Parse("88700d13-f2b6-412a-8a80-3e0d81ff523f"), PrioridadTarea = Prioridad.Media, Titulo = "Pago de servicios públicos", FechaCreacion = DateTime.Now, CategoriaId = Guid.Parse("39275625-39fc-4f59-b678-0d197827370c") });
        tareasInit.Add(new Tarea { TareaId = Guid.Parse("77629269-dc99-4985-bb27-cd3ef2b0cd30"), PrioridadTarea = Prioridad.Baja, Titulo = "Terminar de ver película en Netflix", FechaCreacion = DateTime.Now, CategoriaId = Guid.Parse("39275625-39fc-4f59-b678-0d197827370c") });

        modelBuilder.Entity<Tarea>(tarea =>{
            tarea.ToTable("Tarea");
            tarea.HasKey(p => p.TareaId);

            tarea.Property(p => p.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(p => p.Descripcion).IsRequired(false);
            tarea.Property(p => p.FechaCreacion);
            tarea.Property(p => p.PrioridadTarea);
            tarea.Ignore(p => p.Resumen);

            tarea.HasOne(p => p.Categoria).WithMany(p => p.Tareas).HasForeignKey(p => p.CategoriaId);

            tarea.HasData(tareasInit);
        });
    }
}
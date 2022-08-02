using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectef;
using projectef.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<TareasContext>(o => o.UseInMemoryDatabase("TareasDB"));
builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) =>
{
  dbContext.Database.EnsureCreated(); // Nos asegura si la BBDD está creada y sino la crea
  return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());
});

app.MapGet("api/tareas", async ([FromServices] TareasContext dbContext) =>
{
  var tareas = await dbContext.Tareas.Include(p => p.Categoria).ToListAsync();
  return Results.Ok(tareas);
});

app.MapGet("api/Categorias", async ([FromServices] TareasContext dbContext) =>
{
  var tareas = await dbContext.Categorias.ToListAsync();
  return Results.Ok(tareas);
});

app.MapPost("api/tareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea) =>
{
  tarea.TareaId = Guid.NewGuid();
  tarea.FechaCreacion = DateTime.Now;
  await dbContext.AddAsync(tarea);

  await dbContext.SaveChangesAsync();

  return Results.Ok();
});

app.MapPut("api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea, [FromRoute] Guid id) =>
{
  var tareaDB = await dbContext.Tareas.FindAsync(id);
  if (tareaDB == null)
  {
    return Results.NotFound();
  }
  
  tareaDB.CategoriaId = tarea.CategoriaId;
  tareaDB.Descripcion = tarea.Descripcion;
  tareaDB.Titulo = tarea.Titulo;
  tareaDB.PrioridadTarea = tarea.PrioridadTarea;
  tareaDB.Completada = tarea.Completada;

  await dbContext.SaveChangesAsync();

  return Results.Ok();
});

app.MapDelete("api/Tareas/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id) =>
{
  var tareaDB = await dbContext.Tareas.FindAsync(id);
  if (tareaDB == null)
  {
    return Results.NotFound();
  }
  dbContext.Remove(tareaDB);
  await dbContext.SaveChangesAsync();
  return Results.Ok();
});

app.Run();

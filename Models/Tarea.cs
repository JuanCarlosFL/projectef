using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectef.Models;

public class Tarea
{
    // [Key]
    public Guid TareaId { get; set; }
    // [Required]
    // [MaxLength(200)]
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public Prioridad PrioridadTarea { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Completada { get; set; }
    // [ForeignKey("CategoriaId")]
    public Guid CategoriaId { get; set; } // Relación que la tarea tiene con categoría
    public virtual Categoria Categoria { get; set; } 
    // [NotMapped]
    public string Resumen { get; set; }
}

public enum Prioridad
{
    Baja,
    Media,
    Alta
} 
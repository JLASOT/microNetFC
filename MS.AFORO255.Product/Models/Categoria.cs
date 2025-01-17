using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MS.AFORO255.Product.Models
{
    [Table("categoria")]  // Mapea a la tabla 'categoria' en la base de datos
    public class Categoria
    {
        [Key]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(40)]
        [Column("nombre")]
        public string Nombre { get; set; }
    }
}

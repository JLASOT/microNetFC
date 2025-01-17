using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MS.AFORO255.Product.Models
{
    [Table("producto")]  // Mapea a la tabla 'producto' en la base de datos
    public class Producto
    {
        [Key]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("precio")]
        public float Precio { get; set; }

        [Required]
        [Column("stock")]
        public int Stock { get; set; }



        // Definimos la relación con la tabla 'Categoria'
        [ForeignKey("Categoria")]
        [Column("id_categoria")] // Especificamos que la clave foránea en Producto es 'IdCategoria'
        public int IdCategoria { get; set; }

        // Propiedad de navegación para acceder a la categoría asociada
        public Categoria Categoria { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MS.AFORO255.Sale.Models
{
    [Table("sale_detail")]
    public class SaleDetail
    {
        [Key, Column("sale_detail_id")]
        public int SaleDetailId { get; set; }
        [Column("sale_id")]
        public int SaleId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }
        [ForeignKey("Sale")]
        [Column("sale_price")]
        public double SalePrice { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [JsonIgnore]
        public Sale Sale { get; set; }
    }

}
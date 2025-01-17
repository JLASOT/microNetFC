

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MS.AFORO255.Sale.Models
{
    [Table("sale")]
    public class Sale
    {
        //[Key]
        [Column("sale_id")] // Mapea la columna correctamente
        public int SaleId { get; set; }
        [Column("sale_date")] // Mapea la columna correctamente
        public DateTime SaleDate { get; set; }
        [Column("total_amount")] // Mapea la columna correctamente
        public double TotalAmount { get; set; }
        [Column("customer_id")] // Mapea la columna correctamente
        public int CustomerId { get; set; }

        // Relación con SaleDetail
        //[JsonIgnore]
        public List<SaleDetail> SaleDetails { get; set; }
    }
}

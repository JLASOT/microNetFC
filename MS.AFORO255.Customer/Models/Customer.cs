
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.AFORO255.Customer.Models
{
    [Table("customer")]
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("p_surname")]
        public string PSurname { get; set; }
        [Column("m_surname")]
        public string MSurname { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("company")]
        public string Company { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("type")]
        public string Type { get; set; }     
    }
}

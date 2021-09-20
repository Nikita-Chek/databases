using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab12_2
{
    [Table("product")]
    public class Product
    {

        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string type { get; set; }
        public int quantity { get; set; }
        [Column("manuf_id")]
        public int? manufId{ get; set; }

        public virtual Manufactory manufactory{ get; set; }
    }
}
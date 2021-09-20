using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab12_2
{
    [Table("manufactory")]
    public class Manufactory
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string coutry { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public Manufactory()
        {
            Products = new List<Product>();
        }

        public Manufactory(Manufactory m)
        {
            this.id = m.id;
            this.name = m.name;
            this.coutry = m.coutry;
        }
    }
}
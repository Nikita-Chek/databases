using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab12_2
{
    [Table("client")]
    public class Client
    {
        [Key]
        public int id { get; set; } 
        [Required] 
        public string name { get; set; }
    }
}
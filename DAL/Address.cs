using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public int ClientId { get; set; }
        public int Type { get; set; }
        public string AddressDetail { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("Type")]
        public virtual AddressType AddressType { get; set; }
    }
}

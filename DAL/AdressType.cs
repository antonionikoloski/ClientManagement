﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AddressType
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}

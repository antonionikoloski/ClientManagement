using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class ClientViewModel
    {
        public int ClientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


        public List<Address> Addresses { get; set; }

        
        public List<AddressTypeViewModel> AddressTypes { get; set; }
    }
}

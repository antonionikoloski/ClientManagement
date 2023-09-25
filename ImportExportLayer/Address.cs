using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ImportExportLayer
{
    public class Address
    {
        [XmlAttribute("Type")]
        public int Type { get; set; }
        [XmlText]
        public string AddressDetail { get; set; }
    }
}

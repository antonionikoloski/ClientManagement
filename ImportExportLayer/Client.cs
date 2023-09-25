using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ImportExportLayer
{
    public class Client
    {
        [XmlAttribute("ID")]
        public int ClientId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlArray("Addresses")]
        [XmlArrayItem("Address")]
        public List<Address> Addresses { get; set; }

        [XmlElement("BirthDate")]
        public DateTime BirthDate { get; set; }
    }
}

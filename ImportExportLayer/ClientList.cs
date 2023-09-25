using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ImportExportLayer
{
    [XmlRoot("Clients")]
    public class ClientList
    {
        [XmlElement("Client")]
        public List<Client> Clients { get; set; }
    }
}

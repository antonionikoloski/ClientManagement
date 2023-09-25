using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ImportExportLayer
{
 

    public class XmlExportService
    {
        public void ExportClientsToXml(IEnumerable<Client> clients, string xmlFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClientList));
            using (StreamWriter writer = new StreamWriter(xmlFilePath))
            {
                ClientList clientList = new ClientList { Clients = clients.ToList() };
                serializer.Serialize(writer, clientList);
            }
        }
    }

}

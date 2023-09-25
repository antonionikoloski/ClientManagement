using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ImportExportLayer
{
  

    public class XmlImportService
    {
        public IEnumerable<Client> ImportClientsFromXml(string xmlFilePath, string xsdFilePath = null)
        {
            
            if (!string.IsNullOrEmpty(xsdFilePath))
            {
                ValidateXmlAgainstSchema(xmlFilePath, xsdFilePath);
            }

            
            XmlSerializer serializer = new XmlSerializer(typeof(ClientList));
            using (StreamReader reader = new StreamReader(xmlFilePath))
            {
                ClientList clientList = (ClientList)serializer.Deserialize(reader);
                return clientList.Clients;
            }
        }

        private void ValidateXmlAgainstSchema(string xmlFilePath, string xsdFilePath)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", xsdFilePath);

            XDocument document = XDocument.Load(xmlFilePath);
            document.Validate(schemas, (o, e) =>
            {
                throw new XmlSchemaValidationException($"XML validation error: {e.Message}");
            });
        }
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using com.paralib.paraquick.qbxml.Serializers;
using System.Xml.Serialization;

namespace com.paralib.paraquick.qbxml
{
    public abstract class Message
    {
        protected abstract QBXML OnRoot();

        public string ToXml()
        {
            string xml;
            var qbxml = OnRoot();

            XmlWriterSettings xs = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8
            };


            using (MemoryStream ms = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(ms, xs))
                {
                    var ser = new QBXMLSerializer();
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    ser.Serialize(xw, qbxml, ns);
                }


                ms.Position = 0;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ms);

                XmlProcessingInstruction pi;
                pi = xmlDoc.CreateProcessingInstruction("qbxml", "version=\"13.0\"");
                xmlDoc.InsertAfter(pi, xmlDoc.FirstChild);

                using (MemoryStream ms2 = new MemoryStream())
                {
                    xmlDoc.Save(ms2);

                    ms2.Position = 0;

                    using (StreamReader sr = new StreamReader(ms2))
                    {
                        xml = sr.ReadToEnd();
                    }
                }

                return xml;
            }
        }

    }
}

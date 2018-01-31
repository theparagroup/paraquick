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
    public abstract class MsgSet
    {
        protected abstract QBXML OnSerialize();
        protected abstract void OnDeserialize(QBXML qbxml);

        public string Serialize()
        {
            string xml;
            var qbxml = OnSerialize();

            XmlWriterSettings xs = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.ASCII
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

        public QBXML Deserialize(string xml)
        {
            var ser = new QBXMLSerializer();

            using (var s = new MemoryStream(Encoding.UTF8.GetBytes(xml ?? "")))
            {
                return Deserialize(s);
            }

        }


        public QBXML Deserialize(Stream stream)
        {
            var ser = new QBXMLSerializer();
            QBXML qbxml = (QBXML)ser.Deserialize(stream);

            OnDeserialize(qbxml);

            return qbxml;
        }

    }
}

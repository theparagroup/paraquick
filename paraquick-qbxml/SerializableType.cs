using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace com.paralib.paraquick.qbxml
{
    public abstract class SerializableType
    {
        protected abstract XmlSerializer GetSerializer();

        public string Serialize()
        {
            XmlWriterSettings xs = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8
            };

            using (MemoryStream ms = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(ms, xs))
                {

                    var ser = GetSerializer();
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    ser.Serialize(xw, this, ns);
                }

                ms.Position = 0;

                using (StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }

}

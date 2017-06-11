using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using com.paralib.paraquick.qbxml;
using com.paralib.paraquick.qbxml.Serializers;

namespace qb_test
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomerAddRqType customerAdd = new CustomerAddRqType();
            customerAdd.CustomerAdd = new CustomerAdd();
            customerAdd.CustomerAdd.FirstName = "John";
            customerAdd.CustomerAdd.Name = "John Smith";
            customerAdd.CustomerAdd.LastName = "Smith";

            QBXMLMsgsRq request = new QBXMLMsgsRq();
            request.onError = QBXMLMsgsRqOnError.stopOnError;
            request.Items = new[] { customerAdd };

            QBXML qbxml = new QBXML() { Items = new[] { request }, ItemsElementName = new[] { ItemsChoiceType99.QBXMLMsgsRq } };

            XmlWriterSettings xs = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8


            };

            string xml;


            using (MemoryStream ms = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(ms, xs))
                {
                    //XmlSerializer ser = new XmlSerializer(typeof(QBXML));

                    var ser = new QBXMLSerializer();
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    ser.Serialize(xw, qbxml, ns);

                    //var ser = new Microsoft.Xml.Serialization.GeneratedAssembly.CustomerAddSerializer();
                    //XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    //ser.Serialize(xw, customerAdd.CustomerAdd, ns);

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

                //xml = xmlDoc.OuterXml;

                //using (StreamReader sr = new StreamReader(ms))
                //{
                //    xml = sr.ReadToEnd();
                //}

            }

            Console.WriteLine(xml);
            File.WriteAllText("addcustomer.xml", xml);
        }
    }
}

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
using System.Reflection;

namespace qb_test
{
    class Program
    {
        static void Main(string[] args)
        {

            Serialize();
            Deserialize();
        }

        static void Deserialize()
        {
            var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("qb_test.SampleResponse.xml");
            var rm = new ResponseMessage();
            var qbxml= rm.Deserialize(s);
            s.Close();

            foreach (var rs in rm)
            {
                Console.WriteLine($"{rs.requestID} : {rs.statusCode}");
            }

        }

        static void Serialize()
        {
            var rm = new RequestMessage(QBXMLMsgsRqOnError.stopOnError);

            CustomerAddRqType customerAdd1 = new CustomerAddRqType();
            customerAdd1.CustomerAdd = new CustomerAdd();
            customerAdd1.CustomerAdd.FirstName = "John";
            customerAdd1.CustomerAdd.Name = "John Smith";
            customerAdd1.CustomerAdd.LastName = "Smith";
            customerAdd1.CustomerAdd.SalesTaxCountrySpecified = true;
            customerAdd1.CustomerAdd.SalesTaxCountry = SalesTaxCountry.Canada;
            rm.Add("1",customerAdd1);

            CustomerAddRqType customerAdd2 = new CustomerAddRqType();
            customerAdd2.CustomerAdd = new CustomerAdd();
            customerAdd2.CustomerAdd.FirstName = "John2";
            customerAdd2.CustomerAdd.Name = "John2 Smith2";
            customerAdd2.CustomerAdd.LastName = "Smith2";
            rm.Add("2",customerAdd2);

            var xml = rm.Serialize();

            Console.WriteLine(xml);
            //File.WriteAllText("addcustomer.xml", xml);

        }

    }
}

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
            var rm = new RequestMessage();

            CustomerAddRqType customerAdd1 = new CustomerAddRqType();
            customerAdd1.CustomerAdd = new CustomerAdd();
            customerAdd1.CustomerAdd.FirstName = "John";
            customerAdd1.CustomerAdd.Name = "John Smith";
            customerAdd1.CustomerAdd.LastName = "Smith";
            rm.Requests.Add(customerAdd1);

            CustomerAddRqType customerAdd2 = new CustomerAddRqType();
            customerAdd2.CustomerAdd = new CustomerAdd();
            customerAdd2.CustomerAdd.FirstName = "John2";
            customerAdd2.CustomerAdd.Name = "John2 Smith2";
            customerAdd2.CustomerAdd.LastName = "Smith2";
            rm.Requests.Add(customerAdd2);

            var xml = rm.ToXml();

            Console.WriteLine(xml);
            //File.WriteAllText("addcustomer.xml", xml);
        }
    }
}

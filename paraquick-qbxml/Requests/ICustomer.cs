using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbxml
{
    public interface ICustomer
    {
        string Name { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string CompanyName { get; set; }
        BillAddress BillAddress { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        TermsRef TermsRef { get; set; }
    }
}

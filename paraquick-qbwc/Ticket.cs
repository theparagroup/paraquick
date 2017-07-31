using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.paralib.paraquick.qbwc
{
    public class Ticket
    {
        /*
            The "zero ticket" is the ticket returned when there is nothing to do, or an error.
        */
        public static readonly Ticket Zero = new Ticket(new Guid("00000000-0000-0000-0000-000000000000"));

        public Ticket()
        {
            Value = Guid.NewGuid();
        }

        public Ticket(Guid value)
        {
            Value = value;
        }

        public Guid Value { protected set; get; }

        //Messages
        //Errors

        //serialize/deserialize from database

    }
}

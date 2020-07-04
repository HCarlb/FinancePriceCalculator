using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Events
{
    public class NegativePriceException : Exception
    {

        public NegativePriceException()
        {
            
        }

        public NegativePriceException(string message)
    : base(message)
        {

        }

        public NegativePriceException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}

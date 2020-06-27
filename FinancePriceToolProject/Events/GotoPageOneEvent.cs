using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Events
{
    public class GotoPageOneEvent
    {
        public object Sender { get; private set; }

        public GotoPageOneEvent(object sender)
        {
            Sender = sender;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Events
{
    public class GotoPageThreeEvent
    {
        public object Sender { get; private set; }

        public GotoPageThreeEvent(object sender)
        {
            Sender = sender;
        }
    }
}

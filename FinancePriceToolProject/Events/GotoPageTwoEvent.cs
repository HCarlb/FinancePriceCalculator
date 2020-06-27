using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Events
{
    public class GotoPageTwoEvent
    {
        public object Sender { get; private set; }

        public GotoPageTwoEvent(object sender)
        {
            Sender = sender;
        }
    }
}

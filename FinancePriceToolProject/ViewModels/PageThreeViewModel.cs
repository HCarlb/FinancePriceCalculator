using Caliburn.Micro;
using FinancePriceToolProject.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.ViewModels
{
    public class PageThreeViewModel : Screen, IHandle<GotoPageThreeEvent>
    {
        private readonly SimpleContainer _container;
        private IEventAggregator _events;

        public PageThreeViewModel(SimpleContainer container, IEventAggregator events)
        {
            _container = container;
            _events = events;
            _events.Subscribe(this);
        }

        public void Handle(GotoPageThreeEvent message)
        {
            //throw new NotImplementedException();
        }
    }
}

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
        #region Fields
        private readonly SimpleContainer _container;
        private IEventAggregator _events;
        #endregion

        #region Constructor
        public PageThreeViewModel(SimpleContainer container, IEventAggregator events)
        {
            _container = container;
            _events = events;
            _events.Subscribe(this);
        }
        #endregion

        #region Methods
        public void Handle(GotoPageThreeEvent message)
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}

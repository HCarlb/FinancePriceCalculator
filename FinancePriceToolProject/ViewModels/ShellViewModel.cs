using Caliburn.Micro;
using FinancePriceToolProject.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<GotoPageOneEvent>, IHandle<GotoPageTwoEvent>
	{

        private readonly SimpleContainer _container;
		private IEventAggregator _events;
		private PageOneViewModel _pageOne;
		private PageTwoViewModel _pageTwo;

		public ShellViewModel(IEventAggregator events, SimpleContainer container, PageOneViewModel startPage , PageTwoViewModel pageTwo)
		{

			_container = container;
			_pageOne = startPage;
			_pageTwo = pageTwo;

			// Subscribe to events sent from the ViewModels
			_events = events;
			_events.Subscribe(this);


			// Load the default start page
			ActivateItem(_pageOne);
		}

		public void Handle(GotoPageOneEvent message)
		{
			ActivateItem(_pageOne);
		}

		public void Handle(GotoPageTwoEvent message)
        {
			ActivateItem(_pageTwo);
		}

    }
}

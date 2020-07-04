﻿using Caliburn.Micro;
using FinancePriceToolProject.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<GotoPageOneEvent>, IHandle<GotoPageTwoEvent>, IHandle<GotoPageThreeEvent>
	{
        #region Fields
        private readonly SimpleContainer _container;
		private IEventAggregator _events;
		private PageOneViewModel _pageOne;
		private PageTwoViewModel _pageTwo;
        private PageThreeViewModel _pageThree;
        #endregion

        #region Properties
        public string FormTitle
		{
			// Dynamic Form Title
			get
			{
				var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
				return $"{versionInfo.ProductName} version {versionInfo.FileVersion} - {versionInfo.LegalCopyright}";
			}
		}
		#endregion

		#region Constructor
		public ShellViewModel(IEventAggregator events, SimpleContainer container, PageOneViewModel startPage , PageTwoViewModel pageTwo, PageThreeViewModel pageThree)
		{

			_container = container;
			_pageOne = startPage;
			_pageTwo = pageTwo;
			_pageThree = pageThree;

			// Subscribe to events sent from the ViewModels
			_events = events;
			_events.Subscribe(this);


			// Load the default start page
			ActivateItem(_pageOne);
		}
        #endregion

        #region Navigation
        public void Handle(GotoPageOneEvent message)
		{
			ActivateItem(_pageOne);
		}
		public void Handle(GotoPageTwoEvent message)
        {
			ActivateItem(_pageTwo);
		}
        public void Handle(GotoPageThreeEvent message)
        {
			ActivateItem(_pageThree);
		}
		#endregion
	}
}

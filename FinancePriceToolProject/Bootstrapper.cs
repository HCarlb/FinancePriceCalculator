using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using FinancePriceToolProject.ViewModels;

namespace FinancePriceToolProject
{
    public class Bootstrapper : BootstrapperBase
    {

        private SimpleContainer _container = new SimpleContainer();         // Caliburn Micros Dependency Injection System - SimpleContainer

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            // Configure the SimpleContainer from Caliburn Micro.

            // Wire up Caliburn Micros Dependency Injection System
            _container.Instance(_container);

            // Register singeltons here. These will only be newed up once for the lifetime of the application.
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();
                //.Singleton<ILoggedInUserModel, LoggedInUserModel>();

            // Register att Interfaces here one by one. These will be newed up each time when something calls for them.
            // Can be called by Constructor Dependency Injection of the container ans the an new instance can be gotten by container.GetInstance<IClass>()
            // Usage:
            // public Constructor(SimpleContainer container)
            // {
            //     _container = container;
            //     var newInstance = _container.GetInstance<IClass>();
            // }
            //_container
            //    .PerRequest<DataAccessLibrary.IDataAccess, DataAccessLibrary.DataAccess>()
            //    .PerRequest<DataAccessLibrary.Data.IUser, DataAccessLibrary.Data.User>()
            //    .PerRequest<DataAccessLibrary.Data.IResource, DataAccessLibrary.Data.Resource>()
            //    .PerRequest<DataAccessLibrary.Data.ILocation, DataAccessLibrary.Data.Location>()
            //    .PerRequest<DataAccessLibrary.Data.ICalendar, DataAccessLibrary.Data.Calendar>()
            //    .PerRequest<IPasswordProcessor, PasswordProcessor>()
            //    .PerRequest<IAboutApplication, AboutApplication>()
            //    .PerRequest<IEmailValidator, EmailValidator>();


            // Register all viewmodel classes with the containter using Reflection.
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();       // Start the main window of the application
        }
    }
}

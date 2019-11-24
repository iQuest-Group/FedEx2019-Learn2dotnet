using System;
using System.Windows;
using Autofac;
using Learn2DotNet.Devices.Application;
using Learn2DotNet.Devices.Domain.DataAccess;
using Learn2DotNet.Devices.Domain.RequestBusModel;
using Learn2DotNet.Devices.InMemoryDal;
using Learn2DotNet.Devices.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Learn2DotNet.Devices.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DevicesWindow : Window
    {
        public DevicesWindow()
        {
            InitializeComponent();

            IContainer container = DependencyResolver.BuildContainer();
            IRequestBus requestBus = container.Resolve<IRequestBus>();
            IDeviceRepository deviceRepository = container.Resolve<IDeviceRepository>();

            DataContext = new DevicesViewModel(deviceRepository, requestBus);
        }
    }
}
using System.Windows;

namespace Learn2DotNet.Devices
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DevicesWindow : Window
    {
        public DevicesWindow()
        {
            InitializeComponent();

            DataContext = new DevicesViewModel();
            // GetAutofacContainer();
        }

        /*private static void GetAutofacContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Learn2DotNetDevicesRequestBus>().As<IRequestBus>();
            builder.RegisterType<AutofacRequestHandlerFactory>().As<IRequestHandlerFactory>();
            builder.RegisterType<DeviceRepository>().As<IDeviceRepository>();
            builder.Build();
        }*/
    }
}
using System;
using Autofac;
using Learn2DotNet.Devices.Application;
using Learn2DotNet.Devices.Domain.DataAccess;
using Learn2DotNet.Devices.Domain.RequestBusModel;
using Learn2DotNet.Devices.InMemoryDal;

namespace Learn2DotNet.Devices.Views
{
    public class DependencyResolver
    {
        public static IContainer BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            ConcreteRegistrationSource registrationSource = new ConcreteRegistrationSource();
            registrationSource.AllowedNamespaces.Add("Learn2DotNet.Devices.Application");
            builder.RegisterSource(registrationSource);

            builder.RegisterType<AutofacServiceProvider>().As<IServiceProvider>().SingleInstance();
            builder.RegisterType<Learn2DotNetDevicesRequestBus>().As<IRequestBus>().SingleInstance();
            builder.RegisterType<AutofacRequestHandlerFactory>().As<IRequestHandlerFactory>().SingleInstance();
            builder.RegisterType<DeviceRepository>().As<IDeviceRepository>();

            IContainer container = builder.Build();

            AutofacServiceProvider autofacServiceProvider = container.Resolve<IServiceProvider>() as AutofacServiceProvider;
            autofacServiceProvider.Container = container;

            return container;
        }
    }
}
using System;
using Learn2DotNet.Devices.Domain.RequestBusModel;
using Microsoft.Extensions.DependencyInjection;

namespace Learn2DotNet.Devices
{
    public class AutofacRequestHandlerFactory : IRequestHandlerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public AutofacRequestHandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public T Create<T>()
        {
            return serviceProvider.GetService<T>();
        }

        public object Create(Type type)
        {
            return serviceProvider.GetService(type);
        }
    }
}
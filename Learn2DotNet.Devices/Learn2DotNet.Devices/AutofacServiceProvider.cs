using System;
using Autofac;

namespace Learn2DotNet.Devices
{
    internal class AutofacServiceProvider : IServiceProvider
    {
        public IContainer Container { get; set; }

        public object GetService(Type serviceType)
        {
            return Container?.Resolve(serviceType);
        }
    }
}
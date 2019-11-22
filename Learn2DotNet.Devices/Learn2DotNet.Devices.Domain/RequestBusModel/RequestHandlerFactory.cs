using System;

namespace Learn2DotNet.Devices.Domain.RequestBusModel
{
    public class RequestHandlerFactory : IRequestHandlerFactory
    {
        public T Create<T>()
        {
            return Activator.CreateInstance<T>();
        }

        public object Create(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
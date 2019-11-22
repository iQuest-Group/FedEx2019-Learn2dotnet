using System;

namespace Learn2DotNet.Devices.Domain.RequestBusModel
{
    public interface IRequestHandlerFactory
    {
        T Create<T>();
        object Create(Type type);
    }
}
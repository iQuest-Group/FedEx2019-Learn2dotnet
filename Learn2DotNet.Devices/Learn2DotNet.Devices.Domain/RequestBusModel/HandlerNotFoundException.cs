using System;

namespace Learn2DotNet.Devices.Domain.RequestBusModel
{
    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException()
            : base("No handler is registered for the specified request.")
        {

        }
    }
}
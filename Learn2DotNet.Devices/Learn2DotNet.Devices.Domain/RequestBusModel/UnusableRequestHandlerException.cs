using System;

namespace Learn2DotNet.Devices.Domain.RequestBusModel
{
    public class UnusableRequestHandlerException : Exception
    {
        public UnusableRequestHandlerException()
            : base("There is no requestHandler for the specified request.")
        {

        }
    }
}
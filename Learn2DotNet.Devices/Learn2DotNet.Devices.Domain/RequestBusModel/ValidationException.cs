using System;

namespace Learn2DotNet.Devices.Domain.RequestBusModel
{
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : base(message)
        {
        }
    }
}
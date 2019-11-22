using Learn2DotNet.Devices.Domain.Exceptions;
using Learn2DotNet.Devices.Domain.Model;
using Learn2DotNet.Devices.Domain.RequestBusModel;

namespace Learn2DotNet.Devices.Application.Devices.CreateDevice
{
    public class CreateDeviceRequest : IValidatableObject
    {
        public Device Device { get; set; }

        public void Validate()
        {
            if (Device == null)
            {
                throw new ValidationException($"The {nameof(Device)} is null");
            }
        }
    }
}
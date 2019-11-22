using System;
using System.Threading.Tasks;
using Learn2DotNet.Devices.Domain.DataAccess;
using Learn2DotNet.Devices.Domain.Model;
using Learn2DotNet.Devices.Domain.RequestBusModel;

namespace Learn2DotNet.Devices.Application.Devices.CreateDevice
{
    public class CreateDeviceRequestHandler : IRequestHandlerWithoutResponse<CreateDeviceRequest>
    {
        private readonly IDeviceRepository deviceRepository;

        public CreateDeviceRequestHandler(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository ?? throw new ArgumentNullException($"The {nameof(deviceRepository)} is null");
        }

        public async Task Handle(CreateDeviceRequest request)
        {
            Device device = request.Device;

            if (device.Id == Guid.Empty)
            {
                device.Id = Guid.NewGuid();
            }

            deviceRepository.Add(device);

            await Task.FromResult(0);
        }
    }
}
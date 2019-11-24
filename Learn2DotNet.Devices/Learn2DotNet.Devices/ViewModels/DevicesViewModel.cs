using System.Collections.Generic;
using System.Linq;
using Learn2DotNet.Devices.Commands;
using Learn2DotNet.Devices.Domain.DataAccess;
using Learn2DotNet.Devices.Domain.Model;
using Learn2DotNet.Devices.Domain.RequestBusModel;

namespace Learn2DotNet.Devices.ViewModels
{
    internal class DevicesViewModel
    {
        public DevicesViewModel(IDeviceRepository deviceRepository, IRequestBus requestBus)
        {
            List<Device> devices = deviceRepository.GetAll();

            Devices = devices
                .Select(x =>
                {
                    PairingCommand pairingCommand = new PairingCommand(requestBus, x);
                    return new DeviceViewModel(x, pairingCommand);
                })
                .ToList();
        }

        public List<DeviceViewModel> Devices { get; }
    }
}
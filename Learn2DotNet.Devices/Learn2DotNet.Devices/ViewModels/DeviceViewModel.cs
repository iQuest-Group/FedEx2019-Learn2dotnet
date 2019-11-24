using Learn2DotNet.Devices.Commands;
using Learn2DotNet.Devices.Domain.Model;

namespace Learn2DotNet.Devices.ViewModels
{
    internal class DeviceViewModel
    {
        public DeviceViewModel(Device device, PairingCommand pairingCommand)
        {
            DeviceState = device.DeviceState;
            DeviceName = device.Name;
            PairingCommand = pairingCommand;
        }

        public string DeviceName { get; }

        public DeviceState DeviceState { get; }

        public PairingCommand PairingCommand { get; }
    }
}
using System.Windows.Input;
using Learn2DotNet.Devices.Domain.Model;

namespace Learn2DotNet.Devices
{
    public class DeviceViewModel
    {
        public string DeviceName { get; set; }
        public DeviceState DeviceState { get; set; }
        public ICommand PairingCommand { get; set; }


        public DeviceViewModel(Device device)
        {
            DeviceState = device.DeviceState;
            DeviceName = device.Name;
            PairingCommand = new PairingCommand(device);
        }
    }
}

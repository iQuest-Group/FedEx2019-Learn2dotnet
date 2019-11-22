using System;
using Learn2DotNet.Devices.Domain.Model;
using System.Collections.Generic;

namespace Learn2DotNet.Devices
{
    public class DevicesViewModel
    {
        public ConnectCommand ConnectCommand { get; } = new ConnectCommand();

        public List<Device> Devices { get; set; } = new List<Device>();

        public DevicesViewModel()
        {
            Devices.Add(new Device {Name = "Aspirator 1", Id = new Guid(), DeviceState = DeviceState.Active});
            Devices.Add(new Device {Name = "Aspirator 2", Id = new Guid(), DeviceState = DeviceState.Inactive});
        }
    }
}
using System;
using Learn2DotNet.Devices.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace Learn2DotNet.Devices
{
    public class DevicesViewModel
    {
        public CreateCommand CreateCommand { get; } = new CreateCommand();

        public List<DeviceViewModel> Devices { get; set; }

        public DevicesViewModel()
        {
            List<Device> devices = new List<Device>
            {
                new Device(16000) { Name = "Aspirator 1", Id = Guid.NewGuid(), DeviceState = DeviceState.Active },
                new Device(16001) { Name = "Aspirator 2", Id = Guid.NewGuid(), DeviceState = DeviceState.Inactive }
            };

            Devices = devices
                .Select(x => new DeviceViewModel(x))
                .ToList();
        }
    }
}
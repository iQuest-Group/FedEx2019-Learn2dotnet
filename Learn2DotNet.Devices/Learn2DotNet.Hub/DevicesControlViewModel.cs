using Learn2DotNet.Hub.Domain.Models;
using System.Collections.Generic;
using System.Windows.Input;

namespace Learn2DotNet.Hub
{
    public class DevicesControlViewModel
    {
        public List<Device> Devices { get; set; } = new List<Device>();
        public ICommand PairingCommand { get; set; } = new PairingCommand();

        public DevicesControlViewModel()
        {
            Devices.Add(new Device {Name = "Aspirator 1"});
            Devices.Add(new Device {Name = "Aspirator 2"});
        }
        
    }
}

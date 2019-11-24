using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Learn2DotNet.Hub.Domain.Models;

namespace Learn2DotNet.Hub.Domain
{
    public class DeviceRepository
    {
        private static ConcurrentBag<Device> Devices { get; } = new ConcurrentBag<Device>();

        public void Add(Device device)
        {
            Devices.Add(device);
        }

        public List<Device> GetAll()
        {
            return Devices.ToList();
        }
    }
}

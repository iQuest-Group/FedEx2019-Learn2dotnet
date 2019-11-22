using System;
using System.Collections.Generic;
using System.Linq;
using Learn2DotNet.Devices.Domain.DataAccess;
using Learn2DotNet.Devices.Domain.Model;

namespace Learn2DotNet.Devices.InMemoryDal
{
    public class DeviceRepository : IDeviceRepository
    {
        private static readonly List<Device> Devices = new List<Device>();

        public List<Device> GetAll()
        {
            return Devices.ToList();
        }

        public void Add(Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (string.IsNullOrEmpty(device.Name))
            {
                throw new ArgumentException(nameof(device.Name));
            }

            Devices.Add(device);
        }

        public void Update(Device device)
        {
            Device dbDevice = Devices
                .FirstOrDefault(x => x.Id == device.Id);

            if (dbDevice == null)
            {
                throw new Exception("The device does not exist");
            }

            if (string.IsNullOrEmpty(device.Name))
            {
                throw new ArgumentNullException(nameof(device));
            }

            dbDevice.Name = device.Name;
            dbDevice.DeviceState = device.DeviceState;
        }

        public void Delete(Guid deviceId)
        {
            if (deviceId == Guid.Empty)
            {
                throw new ArgumentException("The device id should not be empty.", nameof(deviceId));
            }

            Device dbBadge = Devices
                .FirstOrDefault(x => x.Id == deviceId);

            if (dbBadge == null)
            {
                throw new ArgumentNullException(nameof(dbBadge), "The device does not exist");
            }

            Devices.Remove(dbBadge);
        }
    }
}
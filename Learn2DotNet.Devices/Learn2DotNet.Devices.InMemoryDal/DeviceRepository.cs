using System;
using System.Collections.Generic;
using System.Linq;
using Learn2DotNet.Devices.Domain.DataAccess;
using Learn2DotNet.Devices.Domain.Model;

namespace Learn2DotNet.Devices.InMemoryDal
{
    public class DeviceRepository : IDeviceRepository
    {
        private static readonly List<Device> Devices;

        static DeviceRepository()
        {
            Devices = new List<Device>
            {
                new Device(16000) { Name = "Aspirator 1", Id = Guid.NewGuid(), DeviceState = DeviceState.Active },
                new Device(16001) { Name = "Aspirator 2", Id = Guid.NewGuid(), DeviceState = DeviceState.Inactive },
                new Device(16002) { Name = "Bec1", Id = Guid.NewGuid(), DeviceState = DeviceState.Active },
                new Device(16000) { Name = "Bec2", Id = Guid.NewGuid(), DeviceState = DeviceState.Active },
                new Device(16001) { Name = "SmartLock", Id = Guid.NewGuid(), DeviceState = DeviceState.Active },
                new Device(16002) { Name = "Printer", Id = Guid.NewGuid(), DeviceState = DeviceState.Active },
                new Device(16000) { Name = "Scanner", Id = Guid.NewGuid(), DeviceState = DeviceState.Active }
            };
        }

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
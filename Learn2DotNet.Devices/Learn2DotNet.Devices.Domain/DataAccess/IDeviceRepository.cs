using System;
using Learn2DotNet.Devices.Domain.Model;

namespace Learn2DotNet.Devices.Domain.DataAccess
{
    public interface IDeviceRepository : IRepository<Device>
    {
        void Update(Device device);
        void Delete(Guid deviceId);
    }
}
using System;
using System.Windows.Input;
using Learn2DotNet.Devices.Application.Devices.CreateDevice;
using Learn2DotNet.Devices.Domain.Model;
using Learn2DotNet.Devices.InMemoryDal;

namespace Learn2DotNet.Devices
{
    public class CreateCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            CreateDeviceRequestHandler deviceHandler = new CreateDeviceRequestHandler(new DeviceRepository());
            Random random = new Random();
            int port = random.Next(16000, 16100);
            CreateDeviceRequest request = new CreateDeviceRequest()
            {
                Device = new Device(port)
                {
                    Id = Guid.NewGuid(),
                    Name = "Aspirator",
                    DeviceState = DeviceState.Active
                }
            };

            deviceHandler.Handle(request).Wait();
        }

        public event EventHandler CanExecuteChanged;
    }
}
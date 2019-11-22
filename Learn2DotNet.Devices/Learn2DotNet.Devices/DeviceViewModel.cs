using System;
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

    internal class PairingCommand : ICommand
    {
        private readonly Device device;

        public PairingCommand(Device device)
        {
            this.device = device;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            device.EnablePairing();
        }

        public event EventHandler CanExecuteChanged;
    }
}

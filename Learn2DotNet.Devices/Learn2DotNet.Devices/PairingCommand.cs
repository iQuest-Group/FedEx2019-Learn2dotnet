using System;
using System.Windows.Input;
using System.Windows.Threading;
using Learn2DotNet.Devices.Domain.Model;

namespace Learn2DotNet.Devices
{
    public class PairingCommand : ICommand
    {
        private readonly Device device;
        private readonly Dispatcher dispatcher;

        public PairingCommand(Device device)
        {
            this.device = device;
            device.PairingStatusChanged += HandlePairingStatusChanged;
            dispatcher = Dispatcher.CurrentDispatcher;
        }

        private void HandlePairingStatusChanged(object sender, EventArgs e)
        {
            dispatcher.Invoke(OnCanExecuteChanged);
        }

        public bool CanExecute(object parameter)
        {
            return device.PairingStatus == PairingStatus.Off;
        }

        public void Execute(object parameter)
        {
            device.EnablePairing();
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
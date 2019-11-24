using System;
using System.Windows.Input;
using System.Windows.Threading;
using Learn2DotNet.Devices.Domain.Model;
using Learn2DotNet.Devices.Domain.RequestBusModel;

namespace Learn2DotNet.Devices.Commands
{
    public class PairingCommand : ICommand
    {
        private readonly Device device;
        private readonly IRequestBus requestBus;
        private readonly Dispatcher dispatcher;

        public event EventHandler CanExecuteChanged;

        public PairingCommand(IRequestBus requestBus, Device device)
        {
            this.device = device;
            this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

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
            //PairingRequest pairingRequest = new PairingRequest
            //{
            //    Device = device
            //};
            //requestBus.ProcessRequest(pairingRequest);

            device.EnablePairing();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
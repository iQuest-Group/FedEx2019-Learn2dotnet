using Learn2DotNet.Hub.Application;
using System;
using System.Windows.Input;

namespace Learn2DotNet.Hub
{
    public class PairingCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SocketClient pairing = new SocketClient();
            pairing.DevicesChanged += HandleDevicesChanged;
            pairing.Start();
        }

        private void HandleDevicesChanged(object sender, EventArgs e)
        {
            App app = System.Windows.Application.Current as App;
            app.AnnounceDevicesChanged();
        }

        public event EventHandler CanExecuteChanged;
    }
}

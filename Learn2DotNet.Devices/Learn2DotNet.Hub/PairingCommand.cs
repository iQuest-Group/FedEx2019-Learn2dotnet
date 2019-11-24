using Learn2DotNet.Hub.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Learn2DotNet.Hub
{
    public class PairingCommand : ICommand
    {
        private bool canExecute = true;
        private readonly Dispatcher dispatcher;

        public PairingCommand()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            canExecute = false;
            dispatcher.Invoke(OnCanExecuteChanged);

            Task.Run(() =>
            {
                IPAddress ipAddress = IPAddress.Loopback;

                Task[] tasks = Enumerable.Range(16000, 3)
                    .Select(x => Task.Run(() =>
                         {
                             SocketClient socketClient = new SocketClient(ipAddress, x);
                             socketClient.DevicesChanged += HandleDevicesChanged;
                             socketClient.SendToDevice();
                         }))
                    .ToArray();

                Task.WaitAll(tasks);

                canExecute = true;
                dispatcher.Invoke(OnCanExecuteChanged);
            });
        }

        private void HandleDevicesChanged(object sender, EventArgs e)
        {
            App app = System.Windows.Application.Current as App;
            app.AnnounceDevicesChanged();
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

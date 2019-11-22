using System;
using System.Net;
using System.Windows.Input;
using Learn2DotNet.Devices.Application;

namespace Learn2DotNet.Devices
{
    public class ConnectCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SocketListener socketListener = new SocketListener();
            socketListener.StartListening();
        }

        public event EventHandler CanExecuteChanged;
    }
}
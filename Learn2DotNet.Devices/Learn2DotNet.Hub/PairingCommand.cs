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
            var pairing = new PairingUseCase();
            pairing.Start();
        }

        public event EventHandler CanExecuteChanged;
    }
}

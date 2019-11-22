using System;
using System.Windows;
using System.Windows.Navigation;
using Learn2DotNet.Hub.Application;

namespace Learn2DotNet.Hub
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static SocketListener listener { get; set; } = new SocketListener();

        public event EventHandler DevicesChanged;

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
            listener.StartListening();
        }

        public void AnnounceDevicesChanged()
        {
            OnDevicesChanged();
        }

        protected virtual void OnDevicesChanged()
        {
            DevicesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

using System.Windows;

namespace Learn2DotNet.Hub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DevicesControlWindow : Window
    {
        public DevicesControlWindow()
        {
            InitializeComponent();
            DataContext = new DevicesControlViewModel();
        }
    }
}

using System;
using Learn2DotNet.Hub.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using Learn2DotNet.Hub.Domain;

namespace Learn2DotNet.Hub
{
    public class DevicesControlViewModel
    {
        private readonly DeviceRepository deviceRepository;
        private Dispatcher dispatcher;

        public ObservableCollection<Device> Devices { get; set; } = new ObservableCollection<Device>();

        public ICommand PairingCommand { get; set; } = new PairingCommand();

        public DevicesControlViewModel()
        {
            deviceRepository = new DeviceRepository();

            App app = App.Current as App;
            app.DevicesChanged += HandleDevicesChanged;

            UpdateDevicesList();

            dispatcher = Dispatcher.CurrentDispatcher;
        }

        private void HandleDevicesChanged(object sender, EventArgs e)
        {
            dispatcher.Invoke(UpdateDevicesList);
        }

        private void UpdateDevicesList()
        {
            Devices.Clear();

            List<Device> devices = deviceRepository.GetAll();

            foreach (Device device in devices)
                Devices.Add(device);
        }
    }
}

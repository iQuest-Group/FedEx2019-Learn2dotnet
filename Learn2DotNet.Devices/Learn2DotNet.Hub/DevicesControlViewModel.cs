using System;
using Learn2DotNet.Hub.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Learn2DotNet.Hub.Domain;

namespace Learn2DotNet.Hub
{
    public class DevicesControlViewModel
    {
        private readonly DeviceRepository deviceRepository;

        public ObservableCollection<Device> Devices { get; set; } = new ObservableCollection<Device>();

        public ICommand PairingCommand { get; set; } = new PairingCommand();

        public DevicesControlViewModel()
        {
            deviceRepository = new DeviceRepository();

            App app = App.Current as App;
            app.DevicesChanged += HandleDevicesChanged;

            UpdateDevicesList();
        }

        private void HandleDevicesChanged(object sender, EventArgs e)
        {
            UpdateDevicesList();
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

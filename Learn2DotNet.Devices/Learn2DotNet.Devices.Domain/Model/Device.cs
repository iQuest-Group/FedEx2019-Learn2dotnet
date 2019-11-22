using System;

namespace Learn2DotNet.Devices.Domain.Model
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DeviceState DeviceState { get; set; }

        public PairingStatus PairingStatus => socketListener.PairingStatus;

        private readonly SocketListener socketListener;

        public event EventHandler PairingStatusChanged;

        public Device(int port)
        {
            socketListener = new SocketListener(port);
            socketListener.PairingStatusChanged += HandlePairingStatusChanged;
        }

        private void HandlePairingStatusChanged(object sender, EventArgs e)
        {
            OnPairingStatusChanged();
        }

        public void EnablePairing()
        {
            socketListener.StartListening();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Device device))
            {
                return false;
            }

            return Id == device.Id
                   && Name == device.Name
                   && DeviceState == device.DeviceState;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + DeviceState.GetHashCode();
                return hash;
            }
        }

        protected virtual void OnPairingStatusChanged()
        {
            PairingStatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
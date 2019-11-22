using System;

namespace Learn2DotNet.Devices.Domain.Model
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DeviceState DeviceState { get; set; }
        public bool AllowPairing { get; set; }

        private readonly SocketListener socketListener;

        public Device(int port)
        {
            socketListener = new SocketListener(port);
        }

        public void EnablePairing()
        {
            AllowPairing = true;
            socketListener.StartListening();
        }

        public void DisablePairing()
        {
            AllowPairing = false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Device device))
            {
                return false;
            }

            return Id == device.Id
                   && Name == device.Name
                   && DeviceState == device.DeviceState
                   && AllowPairing == device.AllowPairing;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + DeviceState.GetHashCode();
                hash = hash * 23 + AllowPairing.GetHashCode();
                return hash;
            }
        }
    }
}
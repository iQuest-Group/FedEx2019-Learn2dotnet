using System;
using System.Threading.Tasks;
using Learn2DotNet.Devices.Domain;
using Learn2DotNet.Devices.Domain.Model;
using Learn2DotNet.Devices.Domain.RequestBusModel;

namespace Learn2DotNet.Devices.Application.Devices.Pairing
{
    public class PairingRequestHandler : IRequestHandlerWithoutResponse<PairingRequest>
    {
        public Task Handle(PairingRequest request)
        {
            Device device = request.Device;

            SocketListener socketListener = new SocketListener(device.Port);
            socketListener.PairingStatusChanged += HandlePairingStatusChanged;
            return socketListener.StartListening();
        }

        private void HandlePairingStatusChanged(object sender, EventArgs e)
        {
        }
    }
}
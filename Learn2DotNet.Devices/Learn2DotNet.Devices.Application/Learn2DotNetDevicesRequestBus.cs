using Learn2DotNet.Devices.Application.Devices.CreateDevice;
using Learn2DotNet.Devices.Domain.RequestBusModel;

namespace Learn2DotNet.Devices.Application
{
    public class Learn2DotNetDevicesRequestBus : RequestBus
    {
        public Learn2DotNetDevicesRequestBus() { }

        public Learn2DotNetDevicesRequestBus(IRequestHandlerFactory handlerFactory)
            : base(handlerFactory)
        {
            Register<CreateDeviceRequest, CreateDeviceRequestHandler>();
        }
    }
}
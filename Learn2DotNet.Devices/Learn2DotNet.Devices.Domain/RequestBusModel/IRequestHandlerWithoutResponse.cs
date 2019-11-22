using System.Threading.Tasks;

namespace Learn2DotNet.Devices.Domain.RequestBusModel
{
    public interface IRequestHandlerWithoutResponse<in TRequest>
    {
        Task Handle(TRequest request);
    }
}
using System.Threading.Tasks;

namespace Learn2DotNet.Devices.Domain.RequestBusModel
{
    public interface IRequestHandlerWithResponse<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
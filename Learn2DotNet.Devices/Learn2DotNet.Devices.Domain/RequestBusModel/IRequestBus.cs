using System.Threading.Tasks;

namespace Learn2DotNet.Devices.Domain.RequestBusModel
{
    public interface IRequestBus
    {
        Task<TResponse> ProcessRequest<TRequest, TResponse>(TRequest request);
        Task ProcessRequest<TRequest>(TRequest request);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learn2DotNet.Devices.Domain.RequestBusModel
{
    public class RequestBus : IRequestBus
    {
        private readonly IRequestHandlerFactory requestHandlerFactory;
        private readonly Dictionary<Type, Type> handlers = new Dictionary<Type, Type>();

        public RequestBus()
        {
            requestHandlerFactory = new RequestHandlerFactory();
        }

        public RequestBus(IRequestHandlerFactory requestHandlerFactory)
        {
            this.requestHandlerFactory = requestHandlerFactory ?? throw new ArgumentNullException(nameof(requestHandlerFactory));
        }

        public void Register<TRequest, THandler>()
        {
            Type requestType = typeof(TRequest);
            Type requestHandlerType = typeof(THandler);

            if (handlers.ContainsKey(requestType))
                throw new Exception("The type " + requestType.FullName + " is already registered.");

            handlers.Add(requestType, requestHandlerType);
        }

        public async Task<TResponse> ProcessRequest<TRequest, TResponse>(TRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            Type requestType = typeof(TRequest);

            if (!handlers.ContainsKey(requestType))
                throw new HandlerNotFoundException();

            if (request is IValidatableObject validatableRequest)
                validatableRequest.Validate();

            Type requestHandlerType = handlers[requestType];

            if (requestHandlerFactory.Create(requestHandlerType) is IRequestHandlerWithResponse<TRequest, TResponse> requestHandlerWithResponse)
            {
                return await requestHandlerWithResponse.Handle(request);
            }

            else if (requestHandlerFactory.Create(requestHandlerType) is IRequestHandlerWithoutResponse<TRequest> requestHandlerWithoutResponse)
            {
                await requestHandlerWithoutResponse.Handle(request);
                return default;
            }

            else
                throw new UnusableRequestHandlerException();
        }

        public async Task ProcessRequest<TRequest>(TRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            Type requestType = typeof(TRequest);

            if (!handlers.ContainsKey(requestType))
                throw new HandlerNotFoundException();

            if (request is IValidatableObject validatableRequest)
                validatableRequest.Validate();

            Type requestHandlerType = handlers[requestType];

            if (requestHandlerFactory.Create(requestHandlerType) is IRequestHandlerWithoutResponse<TRequest> requestHandlerWithoutResponse)
            {
                await requestHandlerWithoutResponse.Handle(request);
            }

            else if (requestHandlerFactory.Create(requestHandlerType) is IRequestHandlerWithResponse<TRequest, object> requestHandlerWithResponse)
            {
                await requestHandlerWithResponse.Handle(request);
            }

            else throw new UnusableRequestHandlerException();
        }
    }
}
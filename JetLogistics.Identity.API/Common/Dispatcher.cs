using Microsoft.Extensions.DependencyInjection;

namespace JetLogistics.Identity.API.Common
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> DispatchAsync<TResponse>(ICommand<TResponse> command)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            return await handler.HandleAsync((dynamic)command);
        }

        public async Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            return await handler.HandleAsync((dynamic)query);
        }
    }
}

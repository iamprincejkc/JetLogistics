
namespace JetLogistics.Common.Common
{
    public interface IDispatcher
    {
        Task<TResponse> DispatchAsync<TResponse>(ICommand<TResponse> command);
        Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query);
    }
}

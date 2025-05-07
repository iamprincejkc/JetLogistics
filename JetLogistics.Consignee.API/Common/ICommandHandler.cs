namespace JetLogistics.Consignee.API.Common
{
    public interface ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<TResponse> HandleAsync(TCommand command);
    }
}

namespace JetLogistics.Consignee.API.Common
{
    public interface IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        Task<TResponse> HandleAsync(TQuery query);
    }
}

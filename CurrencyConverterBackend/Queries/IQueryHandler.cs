namespace CurrencyConverterBackend.Queries
{
    public interface IQueryHandler<Tquery, TResult>
    {
        Task<TResult> HandleAsync(Tquery query);    
    }
}

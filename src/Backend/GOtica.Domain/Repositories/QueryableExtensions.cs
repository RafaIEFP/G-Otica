namespace GOtica.Domain.Repositories;

public static class QueryableExtensions
{
    extension<T> (IQueryable<T> query)
    {
        public IQueryable<T> Paged(int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}

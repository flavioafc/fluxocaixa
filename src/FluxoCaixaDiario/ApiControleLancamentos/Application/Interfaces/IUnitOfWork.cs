namespace ApiControleLancamentos.Application.Interfaces
{
    public interface IUnitOfWork 
    {
        Task CommitAsync(); // Confirma a transação.
        Task RollbackAsync();
    }
}

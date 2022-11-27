namespace KingMarvel.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        void BeginTransaction();

        void Commit();

        void Dispose();
    }
}

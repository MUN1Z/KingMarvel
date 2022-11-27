using KingMarvel.Database.Contexts;
using KingMarvel.Domain.Interfaces;

namespace KingMarvel.Database.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KingMarvelContext _context;

        private bool _disposed;

        public UnitOfWork(KingMarvelContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _disposed = false;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

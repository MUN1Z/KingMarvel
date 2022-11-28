using KingMarvel.Database.Contexts;
using KingMarvel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KingMarvel.Database.Seed
{
    public class DatabaseManager : IDatabaseManager
    {
        #region private members

        private KingMarvelContext _context;

        #endregion

        #region constructors

        public DatabaseManager(KingMarvelContext context)
        {
            context.Database.EnsureCreated();
            _context = context;
        }

        #endregion

        #region public methods implementations

        public async Task SeedData()
        {
        }

        #endregion
    }
}

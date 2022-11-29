using KingMarvel.Database.Contexts;
using KingMarvel.Domain.Entities;

namespace KingMarvel.IntegrationTests.Managers
{
    public class DatabaseTestManager : IDatabaseTestManager
    {
        #region private members

        private KingMarvelContext _kingMarvelContext;

        private const int TEST_BASE_QUANTITY = 5;

        private static bool _hasInstanciated = false;

        #endregion

        #region constructors

        public DatabaseTestManager(KingMarvelContext kingMarvelContext)
        {
            _kingMarvelContext = kingMarvelContext;

            if (_hasInstanciated)
                ClearData();

            _hasInstanciated = true;
        }

        #endregion

        #region public methods implementations

        public async Task SeedTestData()
        {
            await SeedFavoriteCharacters();
        }

        #endregion

        #region private methods implementation

        private void ClearData()
        {
            if (_kingMarvelContext.Character.Any())
                _kingMarvelContext.Character.RemoveRange(_kingMarvelContext.Character);

            _kingMarvelContext.SaveChanges();
        }

        private async Task SeedFavoriteCharacters()
        {
            if (!_kingMarvelContext.Character.Any())
            {
                for (int i = 0; i < TEST_BASE_QUANTITY; i++)
                {
                    var character = new Character
                    {
                        Id = TEST_BASE_QUANTITY,
                        Name = $"Character {TEST_BASE_QUANTITY}",
                        Description = "Description",
                        Favorite = true,
                        Guid = Guid.NewGuid(),
                        Thumb = "http://thumg.png"
                    };

                    await _kingMarvelContext.AddAsync(character);
                    await _kingMarvelContext.SaveChangesAsync();
                }
            }
        }
        #endregion
    }
}

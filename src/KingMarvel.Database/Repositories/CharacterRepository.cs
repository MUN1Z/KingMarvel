using KingMarvel.Database.Contexts;
using KingMarvel.Domain.Entities;
using KingMarvel.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KingMarvel.Database.Repositories
{
    public class CharacterRepository : BaseRepository<Character>, ICharacterRepository
    {
        public CharacterRepository(KingMarvelContext context) : base(context)
        {
        }

        public async Task<Character> GetByIdAsync(int id)
            => await Db.Character.FirstOrDefaultAsync(c => c.Id == id);
    }
}

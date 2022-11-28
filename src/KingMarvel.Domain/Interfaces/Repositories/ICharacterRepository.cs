using KingMarvel.Domain.Entities;

namespace KingMarvel.Domain.Interfaces.Repositories
{
    public interface ICharacterRepository : IBaseRepository<Character>
    {
        Task<Character> GetByIdAsync(int id);
    }
}

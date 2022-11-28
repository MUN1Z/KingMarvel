using KingMarvel.Application.Filters;
using KingMarvel.Application.ViewModels.Response;

namespace KingMarvel.Application.Services.Interfaces
{
    public interface ICharacterService : IDisposable
    {
        Task<IEnumerable<CharacterResponseViewModel>> GetAll(CharacterFilter filter);
        Task<CharacterResponseViewModel> Favorite(CharacterRequestViewModel characterViewModel);
    }
}

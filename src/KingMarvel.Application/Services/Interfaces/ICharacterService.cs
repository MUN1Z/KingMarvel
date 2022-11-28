using KingMarvel.Application.ViewModels.Response;

namespace KingMarvel.Application.Services.Interfaces
{
    public interface ICharacterService : IDisposable
    {
        Task<IEnumerable<CharacterResponseViewModel>> GetAll();
        Task<CharacterResponseViewModel> Favorite(CharacterRequestViewModel characterViewModel);
    }
}

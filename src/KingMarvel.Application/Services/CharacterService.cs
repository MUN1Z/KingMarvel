using AutoMapper;
using KingMarvel.Application.Exceptions;
using KingMarvel.Application.Services.Interfaces;
using KingMarvel.Application.ViewModels.Response;
using KingMarvel.Domain.Entities;
using KingMarvel.Domain.Interfaces;
using KingMarvel.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace KingMarvel.Application.Services
{
    public class CharacterService : BaseService, ICharacterService
    {
        #region private members

        private readonly IConfiguration _configuration;

        private readonly ICharacterRepository _characterRepository;

        #endregion private members

        #region constructors

        public CharacterService(
            IConfiguration configuration,
            ICharacterRepository characterRepository,
            IUnitOfWork work,
            IMapper mapper) : base(work, mapper)
        {
            _configuration = configuration;
            _characterRepository = characterRepository;
        }

        #endregion constructors

        #region public methods implementations

        public async Task<IEnumerable<CharacterResponseViewModel>> GetAll()
        {
            var characters = new List<CharacterResponseViewModel>();

            var ts = DateTime.Now.Ticks.ToString();
            var publicKey = _configuration.GetSection("MarvelAPI").GetSection("PublicKey").Value;
            var privateKey = _configuration.GetSection("MarvelAPI").GetSection("PrivateKey").Value;
            var hash = CreateMD5(ts, publicKey, privateKey);
            //var url = $"http://gateway.marvel.com/v1/public/characters?ts={ts}&apikey={publicKey}&hash={hash}&name={Uri.EscapeUriString("Captain America")}";
            var url = $"http://gateway.marvel.com/v1/public/characters?ts={ts}&apikey={publicKey}&hash={hash}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
                foreach (var item in responseData.data.results)
                {
                    var existentCharacter = await _characterRepository.GetByIdAsync((int)item.id);
                    characters.Add(new CharacterResponseViewModel
                    {
                        Id = item.id,
                        Name = item.name,
                        Description = item.description,
                        Favorite = existentCharacter != null ? existentCharacter.Favorite : false,
                        Thumb = $"{item.thumbnail.path}.{item.thumbnail.extension}"
                    });
                }
            }

            return characters;
        }

        public async Task<CharacterResponseViewModel> Favorite(CharacterRequestViewModel characterViewModel)
        {
            BeginTransaction();
            
            var existentCharacter = await _characterRepository.GetByIdAsync(characterViewModel.Id);

            if (existentCharacter != null)
            {
                await _characterRepository.RemoveAsync(existentCharacter);
                Commit();

                characterViewModel.Favorite = false;
                return _mapper.Map<CharacterResponseViewModel>(characterViewModel);
            }

            var allFavorites = await _characterRepository.FindAllByAsync(c => c.Favorite);

            if (allFavorites.Count() == 5)
                throw new KingMarvelException(Resources.Resource.limit_favorite_exception);

            characterViewModel.Favorite = true;
            var character = _mapper.Map<Character>(characterViewModel);
            await _characterRepository.AddAsync(character);

            Commit();

            return _mapper.Map<CharacterResponseViewModel>(characterViewModel);
        }

        #endregion public methods implementations

        #region private methods implementarions

        private string CreateMD5(string ts, string publicKey, string privateKey)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(ts + privateKey + publicKey);
                var bytesHash = md5.ComputeHash(bytes);
                return BitConverter.ToString(bytesHash).ToLower().Replace("-", String.Empty);
            }
        }

        #endregion
    }
}

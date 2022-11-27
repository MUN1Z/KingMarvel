using AutoMapper;
using KingMarvel.Application.Services.Interfaces;
using KingMarvel.Application.ViewModels.Response;
using KingMarvel.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace KingMarvel.Application.Services
{
    public class CharacterService : BaseService, ICharacterService
    {
        #region private members

        private readonly IConfiguration _configuration;

        #endregion private members

        #region constructors

        public CharacterService(
            IConfiguration configuration,
            IUnitOfWork work) : base(work, null)
        {
            _configuration = configuration;
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
                    characters.Add(new CharacterResponseViewModel
                    {
                        Id = item.id,
                        Name = item.name,
                        Description = item.description,
                        Favorite = false,
                        Thumb = $"{item.thumbnail.path}.{item.thumbnail.extension}"
                    });
                }
            }

            return characters;
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

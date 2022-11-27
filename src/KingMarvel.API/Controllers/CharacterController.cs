using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KingMarvel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : BaseController
    {
        public class Character
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Thumb { get; set; }
            public bool Favorite { get; set; }
        }

        private readonly ILogger<CharacterController> _logger;
        private readonly IConfiguration _config;

        public CharacterController(
            ILogger<CharacterController> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var characters = new List<Character>();

            var ts = DateTime.Now.Ticks.ToString();
            var publicKey = _config.GetSection("MarvelAPI").GetValue<string>("PublicKey");
            var privateKey = _config.GetSection("MarvelAPI").GetValue<string>("PrivateKey");
            var hash = CreateMD5(ts, publicKey, privateKey);
            //var url = $"http://gateway.marvel.com/v1/public/characters?ts={ts}&apikey={publicKey}&hash={hash}&name={Uri.EscapeUriString("Captain America")}";
            var url = $"http://gateway.marvel.com/v1/public/characters?ts={ts}&apikey={publicKey}&hash={hash}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<Root>(url);

                foreach (var item in response.Data.Results)
                {
                    characters.Add(new Character
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Favorite = false,
                        Thumb = $"{item.Thumbnail.Path}.{item.Thumbnail.Extension}"
                    });
                }
            }

            return Response(characters);
        }

        string CreateMD5(string ts, string publicKey, string privateKey)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(ts + privateKey + publicKey);
                var bytesHash = md5.ComputeHash(bytes);
                return BitConverter.ToString(bytesHash).ToLower().Replace("-", String.Empty);
            }
        }

        public class Comics
        {
            public int Available { get; set; }
            public string CollectionURI { get; set; }
            public List<Item> Items { get; set; }
            public int Returned { get; set; }
        }

        public class Data
        {
            public int Offset { get; set; }
            public int Limit { get; set; }
            public int Total { get; set; }
            public int Count { get; set; }
            public List<Result> Results { get; set; }
        }

        public class Events
        {
            public int Available { get; set; }
            public string CollectionURI { get; set; }
            public List<Item> Items { get; set; }
            public int Returned { get; set; }
        }

        public class Item
        {
            public string ResourceURI { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Modified { get; set; }
            public Thumbnail Thumbnail { get; set; }
            public string ResourceURI { get; set; }
            public Comics Comics { get; set; }
            public Series Series { get; set; }
            public Stories Stories { get; set; }
            public Events Events { get; set; }
            public List<URL> Urls { get; set; }
        }

        public class Root
        {
            public int Code { get; set; }
            public string Status { get; set; }
            public string Copyright { get; set; }
            public string AttributionText { get; set; }
            public string AttributionHTML { get; set; }
            public string Etag { get; set; }
            public Data Data { get; set; }
        }

        public class Series
        {
            public int Available { get; set; }
            public string CollectionURI { get; set; }
            public List<Item> Items { get; set; }
            public int Returned { get; set; }
        }

        public class Stories
        {
            public int Available { get; set; }
            public string CollectionURI { get; set; }
            public List<Item> Items { get; set; }
            public int Returned { get; set; }
        }

        public class Thumbnail
        {
            public string Path { get; set; }
            public string Extension { get; set; }
        }

        public class URL
        {
            public string Type { get; set; }
            public string Url { get; set; }
        }
    }
}
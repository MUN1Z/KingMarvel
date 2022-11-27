using KingMarvel.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KingMarvel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : BaseController
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Response(await _characterService.GetAll());
    }
}
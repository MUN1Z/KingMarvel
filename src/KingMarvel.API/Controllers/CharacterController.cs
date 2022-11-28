using KingMarvel.Application.Services.Interfaces;
using KingMarvel.Application.ViewModels.Response;
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

        [HttpPost]
        public async Task<IActionResult> Favorite([FromBody] CharacterRequestViewModel characterViewModel)
        {
            var result = await _characterService.Favorite(characterViewModel);

            if (result != null)
                return Response(result);

            return StatusCode(200);
        }
    }
}
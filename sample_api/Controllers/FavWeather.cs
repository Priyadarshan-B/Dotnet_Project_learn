using Microsoft.AspNetCore.Mvc;
using sample_api.Models;
using System.Threading.Tasks;
using sample_api.Services;

namespace sample_api.Controllers{
    [Route("api/fav")]
    [ApiController]

    public class FavWeather : ControllerBase{
        private readonly FavoritesService _favoritesService;
        public FavWeather(FavoritesService favoriteService){
            _favoritesService = favoriteService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateFavorites([FromBody] UserFavorite userFavorite)
        {
            if (userFavorite == null || string.IsNullOrEmpty(userFavorite.UserId))
            {
                return BadRequest("Invalid user favorite data.");
            }

            await _favoritesService.CreateOrUpdateFavorites(userFavorite);
            return Ok(userFavorite);
        }

         [HttpGet("{user}")]
        public async Task<IActionResult> GetFavorites(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            var cities = await _favoritesService.GetFavoritesByUserId(userId);
            if (cities == null)
            {
                return NotFound("User not found.");
            }
            return Ok(cities);
        }
    }
}
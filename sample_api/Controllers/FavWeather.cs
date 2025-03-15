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

         [HttpPost("get-favorites")]
        public async Task<IActionResult> GetFavorites([FromBody] UserRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest("UserId is required.");
            }

            var cities = await _favoritesService.GetFavoritesByUserId(request.UserId);
            if (cities == null || cities.Count == 0)
            {
                return NotFound("No favorite cities found.");
            }
            return Ok(cities);
        }

        public class UserRequest
        {
            public string UserId { get; set; } = string.Empty;
        }
    }
}
using MongoDB.Driver;
using sample_api.Models;
using Microsoft.Extensions.Configuration;


public class FavoritesService
{
    private readonly IMongoCollection<UserFavorite> _collection;

    public FavoritesService(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("sample_login");
        
        _collection = database.GetCollection<UserFavorite>("UserFavorites");
    }

    public async Task CreateOrUpdateFavorites(UserFavorite userFavorite)
    {
        var filter = Builders<UserFavorite>.Filter.Eq(u => u.User, userFavorite.User);
        var options = new ReplaceOptions { IsUpsert = true };
        await _collection.ReplaceOneAsync(filter, userFavorite, options);
    }

    public async Task<List<string>> GetFavoritesByUserId(string user)
    {
        var filter = Builders<UserFavorite>.Filter.Eq(u => u.User, user);
        var userFavorite = await _collection.Find(filter).FirstOrDefaultAsync();
        return userFavorite?.Cities;
    }
}

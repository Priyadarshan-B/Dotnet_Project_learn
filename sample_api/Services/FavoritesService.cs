using MongoDB.Driver;
using MongoDB.Bson; 
using sample_api.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        var filter = Builders<UserFavorite>.Filter.Eq(u => u.UserId, userFavorite.UserId);

        var update = Builders<UserFavorite>.Update
            .AddToSetEach(u => u.Cities, userFavorite.Cities); 

        var options = new FindOneAndUpdateOptions<UserFavorite>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var updatedFav = await _collection.FindOneAndUpdateAsync(filter, update, options); 

        if (updatedFav == null)
        {
            // userFavorite.Id = ObjectId.GenerateNewId().ToString(); 
            // await _collection.InsertOneAsync(userFavorite);
            updatedFav = await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }

    public async Task<List<string>> GetFavoritesByUserId(string userId)
    {
        var filter = Builders<UserFavorite>.Filter.Eq(u => u.UserId, userId);
        var userFavorite = await _collection.Find(filter).FirstOrDefaultAsync();
        return userFavorite?.Cities ?? new List<string>(); 
    }
}

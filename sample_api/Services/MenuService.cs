using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Collections.Generic;
using  sample_api.Models;

public class MenuService
{
    private readonly IMongoCollection<RoleItem> _menuCollection;
    public  MenuService(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("sample_login");
        _menuCollection = database.GetCollection<RoleItem>("role_resource");
    }

    public async Task<List<MenuItem>> GetResourcesByRole(string role)
    {
        try
        {
            var roleMenu = await _menuCollection.Find(r => r.Role == role).FirstOrDefaultAsync();
            //Console.WriteLine(role,  roleMenu);
            //Console.WriteLine(roleMenu?.Menu);
            return roleMenu?.Menu ?? new List<MenuItem>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching resources by role", ex);
        }

    }
}
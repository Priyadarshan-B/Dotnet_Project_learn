using System;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using sample_api.Models;
using BCrypt.Net;

namespace sample_api.Services
{
    public class AuthServices
    {
        private readonly IMongoCollection<User> _users;

        public AuthServices(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            var database = client.GetDatabase("sample_login");
            _users = database.GetCollection<User>("users");
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        public class UserDTO
        {
            public string Id {get;set;} = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;

        }
        public UserDTO? Authenticate(string username, string password)
        {

            var user = _users.Find(u => u.Username == username).FirstOrDefault();
            if ((user == null) || !VerifyPassword(password, user.Password))
            {
                return null;
            }
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public User Register(User user)
        {
            if (UserExists(user.Username))
            {
                throw new Exception("User already exists");
            }

            user.Password = HashPassword(user.Password);

            _users.InsertOne(user);
            return user;

        }

        private bool UserExists(string username)
        {
            var existingUser = _users.Find(u => u.Username == username).FirstOrDefault();
            return existingUser != null;

        }
    }
}

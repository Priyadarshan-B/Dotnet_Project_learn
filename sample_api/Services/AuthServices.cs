using System;
using MongoDB.Driver;
//using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using sample_api.Models;
using BCrypt.Net;

namespace sample_api.Services
{
    public class AuthServices
    {
        //private readonly string _connectionString;
        private readonly IMongoCollection<User> _users;

        public AuthServices(IConfiguration configuration)
        {
            //_connectionString = configuration.GetConnectionString("DefaultConnection");
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

        public bool Authenticate(string username, string password)
        {

            var user = _users.Find(u => u.Username == username).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            return VerifyPassword(password, user.Password);
            //using (var connection = new MySqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    string query = "SELECT Password FROM users WHERE Username = @username LIMIT 1";
            //    using (var command = new MySqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("@username", username);
            //        var result = command.ExecuteScalar();
            //        if (result == null)
            //        {
            //            return false;
            //        }
            //        string storedPassword = result.ToString();
            //        return VerifyPassword(password, storedPassword);
            //    }
            //}
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

            //using (var connection = new MySqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    string query = "INSERT INTO users (Username, Password) VALUES (@username, @password); SELECT LAST_INSERT_ID();";
            //    using (var command = new MySqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("@username", user.Username);
            //        command.Parameters.AddWithValue("@password", hashedPassword);

            //        var id = command.ExecuteScalar();
            //        user.Id = Convert.ToInt32(id);
            //        user.Password = hashedPassword;
            //        return user;
            //    }
            //}
        }

        private bool UserExists(string username)
        {
            var existingUser = _users.Find(u => u.Username == username).FirstOrDefault();
            return existingUser != null;

            //using (var connection = new MySqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    string query = "SELECT COUNT(*) FROM users WHERE Username = @username";
            //    using (var command = new MySqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("@username", username);
            //        var count = Convert.ToInt32(command.ExecuteScalar());
            //        return count > 0;
            //    }
            //}
        }
    }
}

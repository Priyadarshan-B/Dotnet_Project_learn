using System;
using System.Threading.Tasks;
using Supabase;
using sample_api.Models;
using MongoDB.Driver;

namespace sample_api.Services
{
    public class AuthService
    {
        private readonly Client _supabase;
        private readonly IMongoCollection<UserDetails> _users;

        public AuthService(Client supabase, IMongoClient mongoClient)
        {
            _supabase = supabase;
            var database = mongoClient.GetDatabase("sample_login");
            _users = database.GetCollection<UserDetails>("Users");
        }

        public async Task<UserDetails?> RegisterUser(string username, string email, string password, string phone)
        {
            try
            {
                var response = await _supabase.Auth.SignUp(email, password);

                if (response.User == null)
                    return null;

                 var supabaseId = response.User.Id;
                var supabaseIdString = supabaseId.ToString();


                var existingUser = await _users.Find(u => u.SupabaseId == supabaseIdString).FirstOrDefaultAsync();
                if (existingUser != null)
                    throw new Exception("User already registered.");

                var userDetails = new UserDetails
                {
                    SupabaseId = supabaseIdString,
                    Username = username,
                    Phone = phone
                };
                await _users.InsertOneAsync(userDetails);
                return userDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during registration", ex);
            }
        }

        public async Task<UserResponse?> Login(string email, string password)
        {
            try
            {
                var session = await _supabase.Auth.SignIn(email, password);

                if (session?.User == null)
                {
                    return null;
                }
                var supabaseId = session.User.Id;
                var supabaseIdString = supabaseId.ToString();

                var userDetails = await _users.Find(u => u.SupabaseId == supabaseIdString).FirstOrDefaultAsync();

                if (userDetails != null)
                {
                    return new UserResponse
                    {
                        Id = supabaseIdString, 
                        Username = userDetails.Username,
                        Email = session.User.Email,
                        Phone = userDetails.Phone
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during login", ex);
            }
        }
    }
}

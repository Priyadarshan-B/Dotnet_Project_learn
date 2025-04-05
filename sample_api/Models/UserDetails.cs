using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace sample_api.Models
{
    public class UserDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string SupabaseId { get; set; }

        public string Username { get; set; }
        public string Phone { get; set; }
    }
}

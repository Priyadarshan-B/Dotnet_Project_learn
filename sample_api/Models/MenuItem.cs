using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace sample_api.Models
{
    //[BsonCollection("role_resource")]
    public class MenuItem
    {
        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;
        [BsonElement("path")]
        public string Path { get; set; } = string.Empty;
        [BsonElement("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    public class RoleItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("role")]
        public string Role { get; set; } = string.Empty;

        [BsonElement("resources")]
        public List<MenuItem> Menu { get; set; } = new List<MenuItem>();
    }
}

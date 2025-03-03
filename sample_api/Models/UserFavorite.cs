using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace sample_api.Models
{
    public class UserFavorite
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } 
    [Required]
    [StringLength(255)]
    public string User { get; set; }= string.Empty;

    public List<string> Cities { get; set; }
}

}

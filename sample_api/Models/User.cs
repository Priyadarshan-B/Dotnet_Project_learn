using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace sample_api.Models;

[Table("users")]
public class User : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("phone")]
    public string Phone { get; set; } = string.Empty;

    
}

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
//using System.ComponentModel.DataAnnotations;

//namespace sample_api.Models
//{
//    public class User
//    {
//        [BsonId]
//        [BsonRepresentation(BsonType.ObjectId)]
//        //[Key]
//        public string? Id { get; set; }
//        [Required]
//        [StringLength(50)]
//        public string Username { get; set; } = string.Empty;

//        [Required]
//        [StringLength(255)]
//        public string Email { get; set; } = string.Empty;

//        [Required]
//        [StringLength(255)]
//        public string Password { get; set; } = string.Empty;

//        [Required]
//        [StringLength(12)]
//        public string Phone { get; set; } = string.Empty;
//    }
//}

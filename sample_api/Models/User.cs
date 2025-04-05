using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace sample_api.Models;

[Table("users")]
public class User : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; } 

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;
}

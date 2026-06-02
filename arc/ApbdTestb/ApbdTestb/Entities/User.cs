using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApbdTestb.Entities;

[Table("Users")]
public class User
{
    [Key] public int UserId { get; set; }
    [MaxLength(100)] public string Username     { get; set; } = null!;
    [MaxLength(100)] public string Email        { get; set; } = null!;
    [MaxLength(100)] public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public ICollection<Order> Orders { get; set; } = null!;
}

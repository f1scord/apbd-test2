using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupD.Models;

[Table("Player")]
public class Player
{
    [Key] public int PlayerId { get; set; }
    [MaxLength(50)]  public string FirstName { get; set; } = null!;
    [MaxLength(100)] public string LastName  { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public ICollection<PlayerMatch> PlayerMatches { get; set; } = null!;
}

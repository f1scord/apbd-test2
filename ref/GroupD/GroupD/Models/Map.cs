using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupD.Models;

[Table("Map")]
public class Map
{
    [Key] public int MapId { get; set; }
    [MaxLength(50)] public string Name { get; set; } = null!;
    [MaxLength(50)] public string Type { get; set; } = null!;

    public ICollection<Match> Matches { get; set; } = null!;
}

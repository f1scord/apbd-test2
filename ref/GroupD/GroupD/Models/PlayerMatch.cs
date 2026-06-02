using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GroupD.Models;

[Table("Player_Match")]
[PrimaryKey(nameof(MatchId), nameof(PlayerId))]
public class PlayerMatch
{
    [ForeignKey(nameof(Match))]  public int MatchId  { get; set; }
    [ForeignKey(nameof(Player))] public int PlayerId { get; set; }

    public int MVPs { get; set; }
    [Precision(4, 2)] public decimal Rating { get; set; }

    public Player Player { get; set; } = null!;
    public Match  Match  { get; set; } = null!;
}

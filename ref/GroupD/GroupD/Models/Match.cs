using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GroupD.Models;

[Table("Match")]
public class Match
{
    [Key] public int MatchId { get; set; }

    [ForeignKey(nameof(Tournament))] public int TournamentId { get; set; }
    [ForeignKey(nameof(Map))]        public int MapId        { get; set; }

    public DateTime MatchDate   { get; set; }
    public int      Team1Score  { get; set; }
    public int      Team2Score  { get; set; }

    [Precision(4, 2)] public decimal? BestRating { get; set; }

    public Tournament Tournament { get; set; } = null!;
    public Map        Map        { get; set; } = null!;

    public ICollection<PlayerMatch> PlayerMatches { get; set; } = null!;
}

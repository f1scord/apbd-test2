using GroupD.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupD.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Tournament>  Tournaments  { get; set; }
    public DbSet<Map>         Maps         { get; set; }
    public DbSet<Match>       Matches      { get; set; }
    public DbSet<Player>      Players      { get; set; }
    public DbSet<PlayerMatch> PlayerMatches { get; set; }

    protected DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tournament>().HasData(new List<Tournament>
        {
            new() { TournamentId = 1, Name = "tournament1", StartDate = DateTime.Today, EndDate = DateTime.Today },
            new() { TournamentId = 2, Name = "tournament2", StartDate = DateTime.Today, EndDate = DateTime.Today },
        });

        modelBuilder.Entity<Map>().HasData(new List<Map>
        {
            new() { MapId = 1, Name = "map1", Type = "type1" },
            new() { MapId = 2, Name = "map2", Type = "type1" },
        });

        modelBuilder.Entity<Match>().HasData(new List<Match>
        {
            new() { MatchId = 1, TournamentId = 1, MapId = 1, MatchDate = DateTime.Today, Team1Score = 5,  Team2Score = 15, BestRating = 5 },
            new() { MatchId = 2, TournamentId = 2, MapId = 2, MatchDate = DateTime.Today, Team1Score = 10, Team2Score = 20, BestRating = null },
        });

        modelBuilder.Entity<Player>().HasData(new List<Player>
        {
            new() { PlayerId = 1, FirstName = "John", LastName = "Doe", BirthDate = DateTime.Today },
            new() { PlayerId = 2, FirstName = "Jane", LastName = "Doe", BirthDate = DateTime.Today },
        });

        modelBuilder.Entity<PlayerMatch>().HasData(new List<PlayerMatch>
        {
            new() { MatchId = 1, PlayerId = 1, MVPs = 5, Rating = 4 },
            new() { MatchId = 2, PlayerId = 1, MVPs = 6, Rating = 5 },
            new() { MatchId = 1, PlayerId = 2, MVPs = 7, Rating = 6 },
            new() { MatchId = 2, PlayerId = 2, MVPs = 8, Rating = 7 },
        });
    }
}

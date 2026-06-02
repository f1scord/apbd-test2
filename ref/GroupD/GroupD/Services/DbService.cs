using GroupD.Data;
using GroupD.DTOs;
using GroupD.Exceptions;
using GroupD.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupD.Services;

public class DbService(DatabaseContext context) : IDbService
{
    public async Task<PlayerDataDto> GetPlayerDataAsync(int id)
    {
        var player = await context.Players
            .Include(p => p.PlayerMatches)
                .ThenInclude(pm => pm.Match)
                    .ThenInclude(m => m.Tournament)
            .Include(p => p.PlayerMatches)
                .ThenInclude(pm => pm.Match)
                    .ThenInclude(m => m.Map)
            .FirstOrDefaultAsync(p => p.PlayerId == id);

        if (player == null)
            throw new NotFoundException($"Player with id {id} not found.");

        return new PlayerDataDto
        {
            PlayerId  = player.PlayerId,
            FirstName = player.FirstName,
            LastName  = player.LastName,
            BirthDate = player.BirthDate,
            Matches   = player.PlayerMatches.Select(pm => new MatchDto
            {
                Tournament = pm.Match.Tournament.Name,
                Map        = pm.Match.Map.Name,
                Date       = pm.Match.MatchDate,
                MVPs       = pm.MVPs,
                Rating     = pm.Rating,
                Team1Score = pm.Match.Team1Score,
                Team2Score = pm.Match.Team2Score,
            }).ToList()
        };
    }

    public async Task CreatePlayerAsync(CreatePlayerRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var player = new Player
            {
                FirstName = request.FirstName,
                LastName  = request.LastName,
                BirthDate = request.BirthDate,
            };
            context.Players.Add(player);

            foreach (var matchDto in request.Matches)
            {
                var match = await context.Matches
                    .FirstOrDefaultAsync(m => m.MatchId == matchDto.MatchId);

                if (match == null)
                    throw new NotFoundException($"Match with id {matchDto.MatchId} not found.");

                var playerMatch = new PlayerMatch
                {
                    Player = player,
                    Match  = match,
                    MVPs   = matchDto.MVPs,
                    Rating = matchDto.Rating,
                };
                context.PlayerMatches.Add(playerMatch);

                if (match.BestRating == null || match.BestRating < matchDto.Rating)
                    match.BestRating = matchDto.Rating;
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

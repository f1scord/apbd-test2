using GroupD.DTOs;

namespace GroupD.Services;

public interface IDbService
{
    Task<PlayerDataDto> GetPlayerDataAsync(int id);
    Task CreatePlayerAsync(CreatePlayerRequest request);
}

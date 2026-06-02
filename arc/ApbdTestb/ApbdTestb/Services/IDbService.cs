using ApbdTestb.DTOs;

namespace ApbdTestb.Services;

public interface IDbService
{
    Task<OrderDto> GetOrderByIdAsync(int id);
    Task UpdateOrderAsync(int id);
}

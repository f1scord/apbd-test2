using ApbdTestf.DTOs;

namespace ApbdTestf.Services;

public interface IDbService
{
    Task<CustomerPurchasesDto> GetCustomerPurchasesAsync(int customerId);
    Task                       CreateCustomerAsync(CreateCustomerRequest request);
}

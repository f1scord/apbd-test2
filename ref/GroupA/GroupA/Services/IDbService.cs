using GroupA.DTOs;

namespace GroupA.Services;

public interface IDbService
{
    Task<CustomerDataDto> GetCustomerDataAsync(int customerId);
    Task CreateCustomerAsync(CreateCustomerRequest request);
}

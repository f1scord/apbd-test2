using ApbdTeste.DTOs;

namespace ApbdTeste.Services;

public interface IDbService
{
    Task<CustomerPurchasesDto>    GetCustomerPurchasesAsync(int customerId);
    Task                          CreateWashingMachineAsync(CreateWashingMachineRequest request);
}

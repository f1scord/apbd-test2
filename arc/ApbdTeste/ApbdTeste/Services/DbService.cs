using ApbdTeste.Data;
using ApbdTeste.DTOs;
using ApbdTeste.Entities;
using ApbdTeste.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApbdTeste.Services;

public class DbService(DatabaseContext context) : IDbService
{
    public async Task<CustomerPurchasesDto> GetCustomerPurchasesAsync(int customerId)
    {
        var customer = await context.Customers
            .Include(c => c.PurchaseHistories)
                .ThenInclude(ph => ph.AvailableProgram)
                    .ThenInclude(ap => ap.WashingMachine)
            .Include(c => c.PurchaseHistories)
                .ThenInclude(ph => ph.AvailableProgram)
                    .ThenInclude(ap => ap.Program)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (customer == null)
            throw new NotFoundException($"Customer with id {customerId} not found.");

        return new CustomerPurchasesDto
        {
            FirstName   = customer.FirstName,
            LastName    = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            Purchases   = customer.PurchaseHistories.Select(ph => new PurchaseDto
            {
                Date   = ph.PurchaseDate,
                Rating = ph.Rating,
                Price  = ph.AvailableProgram.Price,
                WashingMachine = new WashingMachineDto
                {
                    Serial    = ph.AvailableProgram.WashingMachine.SerialNumber,
                    MaxWeight = ph.AvailableProgram.WashingMachine.MaxWeight,
                },
                Program = new ProgramDto
                {
                    Name     = ph.AvailableProgram.Program.Name,
                    Duration = ph.AvailableProgram.Program.DurationMinutes,
                },
            }).ToList(),
        };
    }

    public async Task CreateWashingMachineAsync(CreateWashingMachineRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var duplicate = await context.WashingMachines
                .AnyAsync(w => w.SerialNumber == request.WashingMachine.SerialNumber);

            if (duplicate)
                throw new ConflictException($"Washing machine with serial number '{request.WashingMachine.SerialNumber}' already exists.");

            var washingMachine = new WashingMachine
            {
                MaxWeight    = request.WashingMachine.MaxWeight,
                SerialNumber = request.WashingMachine.SerialNumber,
            };
            context.WashingMachines.Add(washingMachine);

            foreach (var ap in request.AvailablePrograms)
            {
                var program = await context.Programs
                    .FirstOrDefaultAsync(p => p.Name == ap.ProgramName);

                if (program == null)
                    throw new NotFoundException($"Program '{ap.ProgramName}' not found.");

                context.AvailablePrograms.Add(new AvailableProgram
                {
                    WashingMachine = washingMachine,
                    Program        = program,
                    Price          = ap.Price,
                });
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

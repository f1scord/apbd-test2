using ApbdTestb.Data;
using ApbdTestb.DTOs;
using ApbdTestb.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApbdTestb.Services;

public class DbService(DatabaseContext context) : IDbService
{
    public async Task<OrderDto> GetOrderByIdAsync(int id)
    {
        var order = await context.Orders
            .Include(o => o.User)
            .Include(o => o.Payments)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null)
            throw new NotFoundException($"Order with id {id} not found.");

        return new OrderDto
        {
            OrderId     = order.OrderId,
            OrderDate   = order.OrderDate,
            Status      = order.Status,
            TotalAmount = order.TotalAmount,
            User        = order.User.Username,
            Payments    = order.Payments.Select(p => new PaymentDto
            {
                PaymentId     = p.PaymentId,
                PaymentMethod = p.PaymentMethod,
                Amount        = p.Amount,
                PaymentStatus = p.PaymentStatus,
            }).ToList(),
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Quantity = oi.Quantity,
                Price    = oi.Price,
                Product  = new ProductDto
                {
                    ProductId     = oi.Product.ProductId,
                    Name          = oi.Product.Name,
                    Description   = oi.Product.Description,
                    Price         = oi.Product.Price,
                    StockQuantity = oi.Product.StockQuantity,
                }
            }).ToList()
        };
    }

    public async Task UpdateOrderAsync(int id)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var order = await context.Orders
                .Include(o => o.Payments)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                throw new NotFoundException($"Order with id {id} not found.");

            // Cannot update if order has payments
            if (order.Payments.Any())
                throw new ConflictException("Order has existing payments and cannot be updated.");

            // Change status to Processed
            order.Status = "Processed";

            // Reduce each product price by 10% and update order item price
            decimal newTotal = 0;
            foreach (var item in order.OrderItems)
            {
                item.Product.Price = Math.Round(item.Product.Price * 0.9m, 2);
                item.Price         = item.Product.Price;
                newTotal          += item.Price * item.Quantity;
            }

            order.TotalAmount = newTotal;

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

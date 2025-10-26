using BugStore.Api.Abstractions.Handlers.Orders;
using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Orders;

public class Handler(AppDbContext db) : IHandler
{
    public async Task<Responses.Orders.Create> CreateOrderAsync(Requests.Orders.Create request, CancellationToken cancellationToken = default)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            Customer = request.Customer,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Lines = request.Lines
        };

        try
        {
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);
            if (customer is null)
            {
                return new Responses.Orders.Create(null, 404, "Customer not found.");
            }

            order.Customer = customer;

            var products = await db.Products.Where(p => request.Lines.Select(l => l.ProductId).Contains(p.Id)).ToListAsync(cancellationToken);

            foreach (var line in order.Lines)
            {
                var product = products.FirstOrDefault(p => p.Id == line.ProductId);
                if (product is null)
                {
                    return new Responses.Orders.Create(null, 404, $"Product with ID {line.ProductId} not found.");
                }
                line.OrderId = order.Id;
                line.Product = product;
            }

            await db.Orders.AddAsync(order, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return new Responses.Orders.Create(order);
        }
        catch
        {
            return new Responses.Orders.Create(null, 500, "An error occurred while creating the order.");
        }
    }

    public async Task<Responses.Orders.GetById> GetOrderByIdAsync(Requests.Orders.GetById request, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
            return new Responses.Orders.GetById(order);
        }
        catch
        {
            return new Responses.Orders.GetById(null, 500, "An error occurred while retrieving the order.");
        }
    }
}

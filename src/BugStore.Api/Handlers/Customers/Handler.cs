using BugStore.Api.Abstractions.Handlers.Customers;
using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Customers;

public class Handler(AppDbContext db) : IHandler
{
     public async Task<Responses.Customers.Create> CreateCustomerAsync(Requests.Customers.Create request, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                BirthDate = request.BirthDate
            };

            db.Customers.Add(customer);
            await db.SaveChangesAsync(cancellationToken);

            return new Responses.Customers.Create(data: customer);
        }
        catch
        {
            return new Responses.Customers.Create(data: null, statusCode: 500, message: "An error occured while creating the customer.");
        }
    }

    public async Task<Responses.Customers.Delete> DeleteCustomerAsync(Requests.Customers.Delete request, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (customer is null)
            {
                return new Responses.Customers.Delete(data: null, statusCode: 404, message: "Customer not found.");
            }
            db.Customers.Remove(customer);
            await db.SaveChangesAsync(cancellationToken);

            return new Responses.Customers.Delete(data: null, statusCode: 200, message: "Customer deleted successfully.");
        }
        catch
        {
            return new Responses.Customers.Delete(data: null, statusCode: 500, message: "An error occured while deleting the customer.");
        }
    }

    public async Task<Responses.Customers.GetById> GetCustomerByIdAsync(Requests.Customers.GetById request, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await db.Customers
              .AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (customer is null)
            {
                return new Responses.Customers.GetById(data: null, statusCode: 404, message: "Customer not found.");
            }

            return new Responses.Customers.GetById(data: customer);
        }
        catch
        {
            return new Responses.Customers.GetById(data: null, statusCode: 500, message: "An error occured while retrieving the customer.");
        }
    }

    public async Task<Responses.Customers.Get> GetCustomersAsync(Requests.Customers.Get request, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = db.Customers
              .AsNoTracking();

            var customers = await query
              .Take(request.PageSize)
              .Skip((request.PageNumber - 1) * request.PageSize)
              .ToListAsync(cancellationToken);

            var total = await query.CountAsync(cancellationToken);

            return new Responses.Customers.Get(
              data: customers,
              totalCount: total,
              currentPage: request.PageNumber,
              pageSize: request.PageSize);

        }
        catch
        {
            return new Responses.Customers.Get(data: null, statusCode: 500, message: "An error occured while retrieving customers.");
        }
    }

    public async Task<Responses.Customers.Update> UpdateCustomerAsync(Requests.Customers.Update request, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (customer is null)
            {
                return new Responses.Customers.Update(data: null, statusCode: 404, message: "Customer not found.");
            }

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Phone = request.Phone ?? "";
            customer.BirthDate = request.BirthDate;

            db.Customers.Update(customer);
            await db.SaveChangesAsync(cancellationToken);

            return new Responses.Customers.Update(data: customer);
        }
        catch
        {
            return new Responses.Customers.Update(data: null, statusCode: 500, message: "An error occured while updating the customer.");
        }
    }
}
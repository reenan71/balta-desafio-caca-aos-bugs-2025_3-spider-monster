using BugStore.Requests.Customers;

namespace BugStore.Api.Abstractions.Handlers.Customers;

public interface IHandler
{
    Task<BugStore.Responses.Customers.Get> GetCustomersAsync(Get request, CancellationToken cancellationToken = default);
    Task<BugStore.Responses.Customers.GetById> GetCustomerByIdAsync(GetById request, CancellationToken cancellationToken = default);
    Task<BugStore.Responses.Customers.Create> CreateCustomerAsync(Create request, CancellationToken cancellationToken = default);
    Task<BugStore.Responses.Customers.Update> UpdateCustomerAsync(Update request, CancellationToken cancellationToken = default);
    Task<BugStore.Responses.Customers.Delete> DeleteCustomerAsync(Delete request, CancellationToken cancellationToken = default);
}
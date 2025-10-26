using BugStore.Requests.Orders;

namespace BugStore.Api.Abstractions.Handlers.Orders;

public interface IHandler
{
    Task<BugStore.Responses.Orders.GetById> GetOrderByIdAsync(GetById request, CancellationToken cancellationToken = default);
    Task<BugStore.Responses.Orders.Create> CreateOrderAsync(Create request, CancellationToken cancellationToken = default);
}
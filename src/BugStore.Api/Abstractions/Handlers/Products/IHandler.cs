using BugStore.Requests.Products;

namespace BugStore.Api.Abstractions.Handlers.Products;

public interface IHandler
{
    Task<BugStore.Responses.Products.Get> GetProductsAsync(Get request, CancellationToken cancellationToken = default);
    Task<BugStore.Responses.Products.GetById> GetProductByIdAsync(GetById request, CancellationToken cancellationToken = default);
    Task<BugStore.Responses.Products.Create> CreateProductAsync(Create request, CancellationToken cancellationToken = default);
    Task<BugStore.Responses.Products.Update> UpdateProductAsync(Update request, CancellationToken cancellationToken = default);
    Task<BugStore.Responses.Products.Delete> DeleteProductAsync(Delete request, CancellationToken cancellationToken = default);
}
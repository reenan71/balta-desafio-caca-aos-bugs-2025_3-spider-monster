using BugStore.Api.Responses;
using BugStore.Models;

namespace BugStore.Responses.Products;

public class Get : PagedResponse<List<Product>>
{
    public Get(List<Product>? data, int totalCount, int currentPage, int pageSize) : base(data, totalCount, currentPage, pageSize)
    {

    }

    public Get(List<Product>? data, int statusCode = 200, string? message = null) : base(data, statusCode, message)
    {

    }
}
using BugStore.Api.Responses;
using BugStore.Models;

namespace BugStore.Responses.Customers;

public class Get : PagedResponse<List<Customer>>
{
    public Get(List<Customer>? data, int totalCount, int currentPage, int pageSize) : base(data, totalCount, currentPage, pageSize)
    {

    }

    public Get(List<Customer>? data, int statusCode = 200, string? message = null) : base(data, statusCode, message)
    {

    }
}
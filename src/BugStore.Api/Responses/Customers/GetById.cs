using BugStore.Api.Responses;
using BugStore.Models;

namespace BugStore.Responses.Customers;

public class GetById : Response<Customer>
{
    public GetById(Customer? data, int statusCode = 200, string? message = null) : base(data, statusCode, message)
    {

    }
}
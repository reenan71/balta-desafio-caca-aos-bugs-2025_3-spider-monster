using BugStore.Api.Responses;
using BugStore.Models;

namespace BugStore.Responses.Customers;

public class Create : Response<Customer>
{
    public Create(Customer? data, int statusCode = 200, string message = "Request processed successfully.") : base(data, statusCode, message)
    {
    }
}
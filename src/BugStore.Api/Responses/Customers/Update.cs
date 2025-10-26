using BugStore.Api.Responses;
using BugStore.Models;

namespace BugStore.Responses.Customers;

public class Update : Response<Customer>
{
    public Update(Customer? data, int statusCode = 200, string message = "Request processed successfully.") : base(data, statusCode, message)
    {
    }
}
using BugStore.Api.Responses;
using BugStore.Models;

namespace BugStore.Responses.Orders;

public class Create : Response<Order>
{
    public Create(Order? data, int statusCode = 201, string message = "Order created successfully.") : base(data, statusCode, message)
    {
    }
}
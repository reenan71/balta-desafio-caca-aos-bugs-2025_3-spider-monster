using BugStore.Api.Responses;
using BugStore.Models;

namespace BugStore.Responses.Orders;

public class GetById :  Response<Order>
{
    public GetById(Order? data, int statusCode = 200, string message = "Order retrieved successfully.") : base(data, statusCode, message)
    {
    }
}
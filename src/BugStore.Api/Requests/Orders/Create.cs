using BugStore.Api.Requests;
using BugStore.Models;

namespace BugStore.Requests.Orders;

public class Create : Request
{
    public Guid CustomerId { get; set; }
    public required Customer Customer { get; set; }
    public List<OrderLine> Lines { get; set; } = [];
}
using BugStore.Api.Requests;

namespace BugStore.Requests.Customers;

public class GetById : Request
{
    public Guid Id { get; set; }
}
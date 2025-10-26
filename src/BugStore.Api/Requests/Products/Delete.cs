using BugStore.Api.Requests;

namespace BugStore.Requests.Products;

public class Delete : Request
{
    public Guid Id { get; set; }
}
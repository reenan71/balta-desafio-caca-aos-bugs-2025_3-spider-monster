using BugStore.Api.Requests;

namespace BugStore.Requests.Products;

public class Update : Request
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Slug { get; set; }
    public required decimal Price { get; set; }
}
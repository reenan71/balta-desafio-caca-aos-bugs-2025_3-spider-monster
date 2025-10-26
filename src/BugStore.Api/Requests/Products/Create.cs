using BugStore.Api.Requests;

namespace BugStore.Requests.Products;

public class Create : Request
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Slug { get; set; }
    public decimal Price { get; set; }
}
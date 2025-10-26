using BugStore.Api.Requests;

namespace BugStore.Requests.Customers;

public class Update : Request
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public DateTime BirthDate { get; set; }
}
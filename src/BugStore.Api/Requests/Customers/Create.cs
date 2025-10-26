using BugStore.Api.Requests;

namespace BugStore.Requests.Customers;

public class Create : Request
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime BirthDate { get; set; }
}
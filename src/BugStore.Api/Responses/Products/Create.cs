using BugStore.Api.Responses;
using BugStore.Models;

namespace BugStore.Responses.Products;

public class Create : Response<Product>
{
    public Create(Product? data, int statusCode = 201, string message = "Product created successfully.") : base(data, statusCode, message)
    {
    }
}
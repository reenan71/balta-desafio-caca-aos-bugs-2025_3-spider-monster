using BugStore.Api.Responses;
using BugStore.Models;

namespace BugStore.Responses.Products;

public class GetById : Response<Product>
{
    public GetById(Product? data, int statusCode = 200, string message = "Request processed successfully.") : base(data, statusCode, message)
    {
    }  
}
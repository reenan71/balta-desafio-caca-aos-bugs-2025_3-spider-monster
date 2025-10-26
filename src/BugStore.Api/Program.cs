using BugStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddTransient<BugStore.Api.Abstractions.Handlers.Customers.IHandler, BugStore.Handlers.Customers.Handler>();
builder.Services.AddTransient<BugStore.Api.Abstractions.Handlers.Products.IHandler, BugStore.Handlers.Products.Handler>();
builder.Services.AddTransient<BugStore.Api.Abstractions.Handlers.Orders.IHandler, BugStore.Handlers.Orders.Handler>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

#region Customer Endpoints

app.MapPost("/v1/customers", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Customers.IHandler customerHandler,
      BugStore.Requests.Customers.Create request,
      CancellationToken cancellationToken = default) =>
{
    var result = await customerHandler.CreateCustomerAsync(request, cancellationToken);

    return result.IsSuccess
      ? TypedResults.Created($"/v1/customers/{result.Data?.Id}", result)
      : TypedResults.BadRequest();
});

app.MapGet("/v1/customers", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Customers.IHandler customerHandler,
      int page = 1,
      int pageSize = 10,
      CancellationToken cancellationToken = default) =>
{
    var request = new BugStore.Requests.Customers.Get
    {
        PageNumber = page,
        PageSize = pageSize
    };

    var result = await customerHandler.GetCustomersAsync(request, cancellationToken);

    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest();
});

app.MapGet("/v1/customers/{id}", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Customers.IHandler customerHandler,
      Guid id,
      CancellationToken cancellationToken = default) =>
{
    var request = new BugStore.Requests.Customers.GetById
    {
        Id = id
    };

    var result = await customerHandler.GetCustomerByIdAsync(request, cancellationToken);
    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest();
});

app.MapPut("/v1/customers/{id}", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Customers.IHandler customerHandler,
      Guid id,
      BugStore.Requests.Customers.Update request,
      CancellationToken cancellationToken = default) =>
{
    request.Id = id;
    var result = await customerHandler.UpdateCustomerAsync(request, cancellationToken);

    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest();
});

app.MapDelete("/v1/customers/{id}", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Customers.IHandler customerHandler,
      Guid id,
      CancellationToken cancellationToken = default) =>
{
    var request = new BugStore.Requests.Customers.Delete
    {
        Id = id
    };

    var result = await customerHandler.DeleteCustomerAsync(request, cancellationToken);

    return result.IsSuccess
      ? TypedResults.NoContent()
      : TypedResults.NotFound();
});

#endregion

#region Products Endpoints

app.MapPost("/v1/products", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Products.IHandler productHandler,
      BugStore.Requests.Products.Create request,
      CancellationToken cancellationToken = default) =>
{
    var result = await productHandler.CreateProductAsync(request, cancellationToken);

    return result.IsSuccess
      ? TypedResults.Created($"/v1/products/{result.Data?.Id}", result)
      : TypedResults.BadRequest();
});

app.MapGet("/v1/products/{id}", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Products.IHandler productHandler,
      Guid id,
      CancellationToken cancellationToken = default) =>
{
    var request = new BugStore.Requests.Products.GetById
    {
        Id = id
    };

    var result = await productHandler.GetProductByIdAsync(request, cancellationToken);

    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest();
});

app.MapGet("/v1/products", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Products.IHandler handler,
      int pageNumber = 1,
      int pageSize = 10,
      CancellationToken cancellationToken = default) =>
{
    var request = new BugStore.Requests.Products.Get
    {
        PageNumber = pageNumber,
        PageSize = pageSize
    };

    var result = await handler.GetProductsAsync(request, cancellationToken);

    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest();
});

app.MapPut("/v1/products/{id}", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Products.IHandler productHandler,
      Guid id,
      BugStore.Requests.Products.Update request,
      CancellationToken cancellationToken = default) =>
{
    request.Id = id;

    var result = await productHandler.UpdateProductAsync(request, cancellationToken);

    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.NotFound();
});

app.MapDelete("/v1/products/{id}", async Task<IResult> (
      [FromServices] BugStore.Api.Abstractions.Handlers.Products.IHandler productHandler,
      Guid id,
      CancellationToken cancellationToken = default) =>
{
    var request = new BugStore.Requests.Products.Delete
    {
        Id = id
    };

    var result = await productHandler.DeleteProductAsync(request, cancellationToken);

    return result.IsSuccess
      ? TypedResults.NoContent()
      : TypedResults.NotFound();
});

#endregion

#region Orders Endpoints

app.MapPost("/v1/orders", async (
      [FromServices] BugStore.Api.Abstractions.Handlers.Orders.IHandler handler,
      BugStore.Requests.Orders.Create request,
      CancellationToken cancellationToken = default) =>
{
    var result = await handler.CreateOrderAsync(request, cancellationToken);
    return result.IsSuccess
      ? Results.Created($"/orders/{result.Data?.Id}", result)
      : Results.BadRequest();
});

app.MapGet("/orders/{id}", async (
      [FromServices] BugStore.Api.Abstractions.Handlers.Orders.IHandler handler,
      Guid id,
      CancellationToken cancellationToken = default) =>
{
    var request = new BugStore.Requests.Orders.GetById { Id = id };
    var result = await handler.GetOrderByIdAsync(request, cancellationToken);
    return result.IsSuccess
      ? Results.Ok(result)
      : Results.BadRequest();
});

#endregion

app.Run();

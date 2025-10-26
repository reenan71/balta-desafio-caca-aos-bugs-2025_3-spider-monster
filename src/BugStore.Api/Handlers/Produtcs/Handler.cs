using BugStore.Api.Abstractions.Handlers.Products;
using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Products;

public class Handler(AppDbContext db) : IHandler
{
    public async Task<Responses.Products.Create> CreateProductAsync(Requests.Products.Create request, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description ?? "",
            Slug = request.Slug,
            Price = request.Price
        };

        try
        {
            await db.Products.AddAsync(product, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return new Responses.Products.Create(product);
        }
        catch
        {
            return new Responses.Products.Create(null, 500, "An error occurred while creating the product.");
        }
    }

    public async Task<Responses.Products.Delete> DeleteProductAsync(Requests.Products.Delete request, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (product is null)
            {
                return new Responses.Products.Delete(null, 404, "Product not found.");
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync(cancellationToken);

            return new Responses.Products.Delete(data: null);
        }
        catch
        {
            return new Responses.Products.Delete(null, 500, "An error occurred while deleting the product.");
        }
    }

    public async Task<Responses.Products.GetById> GetProductByIdAsync(Requests.Products.GetById request, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product is null)
            {
                return new Responses.Products.GetById(null, 404, "Product not found.");
            }

            return new Responses.Products.GetById(product);
        }
        catch
        {
            return new Responses.Products.GetById(null, 500, "An error occurred while retrieving the product.");
        }
    }

    public async Task<Responses.Products.Get> GetProductsAsync(Requests.Products.Get request, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = db.Products.AsNoTracking();

            var products = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var total = await query.CountAsync(cancellationToken);

            return new Responses.Products.Get(
              data: products,
              totalCount: total,
              currentPage: request.PageNumber,
              pageSize: request.PageSize);

        }
        catch
        {
            return new Responses.Products.Get(null, 500, "An error occurred while retrieving products.");
        }
    }

    public async Task<Responses.Products.Update> UpdateProductAsync(Requests.Products.Update request, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product is null)
            {
                return new Responses.Products.Update(null, 404, "Product not found.");
            }

            product.Title = request.Title;
            product.Description = request.Description ?? "";
            product.Slug = request.Slug;
            product.Price = request.Price;

            db.Products.Update(product);
            await db.SaveChangesAsync(cancellationToken);

            return new Responses.Products.Update(product);
        }
        catch
        {
            return new Responses.Products.Update(null, 500, "An error occurred while updating the product.");
        }
    }
}
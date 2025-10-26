using BugStore.Models;
using Create = BugStore.Requests.Products.Create;
using Delete = BugStore.Requests.Products.Delete;
using Get = BugStore.Requests.Products.Get;
using GetById = BugStore.Requests.Products.GetById;
using Update = BugStore.Requests.Products.Update;

namespace BugStore.Test.UnitTests;

public class ProductHandlerSimpleTests
{
    #region Product Tests

    [Fact]
    public async Task Should_Create_Product()
    {
        var request = new Create
        {
            Title = "Mouse Gamer",
            Description = "Mouse óptico com LED RGB",
            Slug = "mouse-gamer",
            Price = 150.99m
        };

        var handler = new FakeProductHandler();
        var createdProduct = await handler.CreateAsync(request, CancellationToken.None);

        Assert.NotNull(createdProduct.Data);
        Assert.Equal(request.Title, createdProduct.Data?.Title);
    }

    [Fact]
    public async Task Should_Update_Product()
    {
        var handler = new FakeProductHandler();

        var createRequest = new Create
        {
            Title = "Teclado",
            Description = "Teclado mecânico ABNT2",
            Slug = "teclado",
            Price = 299.90m
        };

        var createdProduct = await handler.CreateAsync(createRequest, CancellationToken.None);

        var updateRequest = new Update
        {
            Id = createdProduct.Data!.Id,
            Title = "Teclado RGB",
            Description = "Teclado mecânico RGB com switch azul",
            Slug = "teclado-rgb",
            Price = 349.90m
        };

        var updatedProduct = await handler.UpdateAsync(updateRequest, CancellationToken.None);

        Assert.Equal(updateRequest.Title, updatedProduct.Data?.Title);
        Assert.Equal(updateRequest.Price, updatedProduct.Data?.Price);
    }

    [Fact]
    public async Task Should_Delete_Product()
    {
        var handler = new FakeProductHandler();

        var createRequest = new Create
        {
            Title = "Fone de ouvido",
            Description = "Headset com microfone",
            Slug = "fone",
            Price = 199.90m
        };

        var createdProduct = await handler.CreateAsync(createRequest, CancellationToken.None);

        var deleteRequest = new Delete
        {
            Id = createdProduct.Data!.Id
        };

        var deleteResult = await handler.DeleteAsync(deleteRequest, CancellationToken.None);

        Assert.True(deleteResult.IsSuccess);
    }

    [Fact]
    public async Task Should_Get_Product_By_Id()
    {
        var handler = new FakeProductHandler();

        var createRequest = new Create
        {
            Title = "Monitor 27''",
            Description = "Monitor gamer 144Hz",
            Slug = "monitor-27",
            Price = 1599.99m
        };

        var createdProduct = await handler.CreateAsync(createRequest, CancellationToken.None);

        var getByIdRequest = new GetById
        {
            Id = createdProduct.Data!.Id
        };

        var retrievedProduct = await handler.GetByIdAsync(getByIdRequest, CancellationToken.None);

        Assert.Equal(createdProduct.Data.Id, retrievedProduct.Data?.Id);
    }

    [Fact]
    public async Task Should_Get_Products_List()
    {
        var handler = new FakeProductHandler();

        await handler.CreateAsync(new Create
        {
            Title = "Cadeira Gamer",
            Description = "Cadeira ergonômica",
            Slug = "cadeira-gamer",
            Price = 899.90m
        }, CancellationToken.None);

        var getRequest = new Get
        {
            PageNumber = 1,
            PageSize = 10
        };

        var productsList = await handler.GetAsync(getRequest, CancellationToken.None);

        Assert.NotNull(productsList.Data);
        Assert.True(productsList.Data!.Count > 0);
    }

    #endregion
}

// Classe fake para simular o handler (sem banco de dados)
public class FakeProductHandler
{
    private readonly List<Product> _products = new();

    public Task<Responses.Products.Create> CreateAsync(Requests.Products.Create request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Slug = request.Slug,
            Price = request.Price
        };

        _products.Add(product);
        return Task.FromResult(new Responses.Products.Create(product));
    }

    public Task<Responses.Products.Update> UpdateAsync(Requests.Products.Update request, CancellationToken cancellationToken)
    {
        var product = _products.FirstOrDefault(p => p.Id == request.Id);
        if (product != null)
        {
            product.Title = request.Title;
            product.Description = request.Description;
            product.Slug = request.Slug;
            product.Price = request.Price;
        }

        return Task.FromResult(new Responses.Products.Update(product));
    }

    public Task<Responses.Products.Delete> DeleteAsync(Requests.Products.Delete request, CancellationToken cancellationToken)
    {
        var product = _products.FirstOrDefault(p => p.Id == request.Id);
        if (product != null)
        {
            _products.Remove(product);
            return Task.FromResult(new Responses.Products.Delete(
                data: null,
                statusCode: 200,
                message: "Product deleted"
            ));
        }

        return Task.FromResult(new Responses.Products.Delete(
            data: null,
            statusCode: 404,
            message: "Product not found"
        ));
    }

    public Task<Responses.Products.GetById> GetByIdAsync(Requests.Products.GetById request, CancellationToken cancellationToken)
    {
        var product = _products.FirstOrDefault(p => p.Id == request.Id);
        return Task.FromResult(new Responses.Products.GetById(product));
    }

    public Task<Responses.Products.Get> GetAsync(Requests.Products.Get request, CancellationToken cancellationToken)
    {
        var page = _products
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return Task.FromResult(new Responses.Products.Get(
            data: page,
            currentPage: request.PageNumber,
            pageSize: request.PageSize,
            totalCount: _products.Count
        ));
    }
}

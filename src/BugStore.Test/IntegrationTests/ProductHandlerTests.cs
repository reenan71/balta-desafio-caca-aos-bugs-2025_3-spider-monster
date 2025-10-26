using BugStore.Data;
using BugStore.Handlers.Products;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Create = BugStore.Requests.Products.Create;
using Delete = BugStore.Requests.Products.Delete;
using Get = BugStore.Requests.Products.Get;
using GetById = BugStore.Requests.Products.GetById;
using Update = BugStore.Requests.Products.Update;

namespace BugStore.Test
{
    public class ProductHandlerTests
    {
        private readonly Handler _handler;
        private readonly AppDbContext _context;

        public ProductHandlerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _context.Products.Add(new Product 
            { 
                Id = Guid.NewGuid(), 
                Title = "Sample Product", 
                Description = "A sample product description", 
                Slug = "sample-product", 
                Price = 99.99m 
            });
            _context.SaveChanges();
            _handler = new Handler(_context); 
        }

        [Fact]
        public async Task CreateProductAsync_ValidRequest_ReturnsCreatedProduct()
        {
            // Arrange
            var request = new Create
            {
                Title = "New Product",
                Description = "A new product description",
                Slug = "new-product",
                Price = 149.99m
            };

            // Act
            var result = await _handler.CreateProductAsync(request);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(request.Title, result.Data.Title);
            Assert.Equal(request.Price, result.Data.Price);
        }

        [Fact]
        public async Task GetProductsAsync_ValidPagination_ReturnsPagedProducts()
        {
            // Arrange
            var request = new Get { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.GetProductsAsync(request);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(1, result.Data.Count); 
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task GetProductByIdAsync_ExistingId_ReturnsProduct()
        {
            // Arrange
            var product = _context.Products.First();
            var request = new GetById { Id = product.Id };

            // Act
            var result = await _handler.GetProductByIdAsync(request);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.StatusCode); 
            Assert.Equal(product.Id, result.Data.Id);
        }

        [Fact]
        public async Task UpdateProductAsync_ExistingProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            var product = _context.Products.First();
            var request = new Update
            {
                Id = product.Id,
                Title = "Updated Product",
                Description = "An updated product description",
                Slug = "updated-product",
                Price = 199.99m
            };

            // Act
            var result = await _handler.UpdateProductAsync(request);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.StatusCode); 
            Assert.Equal(request.Title, result.Data.Title);
            Assert.Equal(request.Price, result.Data.Price);
        }

        [Fact]
        public async Task DeleteProductAsync_ExistingId_ReturnsSuccess()
        {
            // Arrange
            var product = _context.Products.First();
            var request = new Delete { Id = product.Id };

            // Act
            var result = await _handler.DeleteProductAsync(request);

            // Assert
            Assert.Equal(200, result.StatusCode); 
            Assert.Equal("Request processed successfully.", result.Message); 
            Assert.Null(_context.Products.Find(product.Id));
        }
    }
}
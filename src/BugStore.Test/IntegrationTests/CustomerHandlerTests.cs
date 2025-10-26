using BugStore.Data;
using BugStore.Handlers.Customers;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Create = BugStore.Requests.Customers.Create;
using Delete = BugStore.Requests.Customers.Delete;
using Get = BugStore.Requests.Customers.Get;
using GetById = BugStore.Requests.Customers.GetById;
using Update = BugStore.Requests.Customers.Update;

namespace BugStore.Test
{
    public class CustomerHandlerTests
    {
        private readonly Handler _handler;
        private readonly AppDbContext _context;

        public CustomerHandlerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _context.Customers.Add(new Customer { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" });
            _context.SaveChanges();
            _handler = new Handler(_context); // Passa o contexto para o Handler
        }

        [Fact]
        public async Task CreateCustomerAsync_ValidRequest_ReturnsCreatedCustomer()
        {
            // Arrange
            var request = new Create
            {
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Phone = "0987654321",
                BirthDate = new DateTime(1995, 1, 1)
            };

            // Act
            var result = await _handler.CreateCustomerAsync(request);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.StatusCode); // Mantido como você especificou
            Assert.Equal(request.Name, result.Data.Name);
            Assert.Equal(request.Email, result.Data.Email);
        }

        [Fact]
        public async Task GetCustomersAsync_ValidPagination_ReturnsPagedCustomers()
        {
            // Arrange
            var request = new Get { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.GetCustomersAsync(request);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.StatusCode); // Mantido como você especificou
            Assert.Equal(1, result.Data.Count); // Um cliente inicial do construtor
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ExistingId_ReturnsCustomer()
        {
            // Arrange
            var customer = _context.Customers.First(); // Usa o cliente inicial do contexto
            var request = new GetById { Id = customer.Id };

            // Act
            var result = await _handler.GetCustomerByIdAsync(request);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(customer.Id, result.Data.Id);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ExistingCustomer_ReturnsUpdatedCustomer()
        {
            // Arrange
            var customer = _context.Customers.First();
            var request = new Update
            {
                Id = customer.Id,
                Name = "John Doe Updated",
                Email = "john.updated@example.com",
                Phone = "0987654321",
                BirthDate = new DateTime(1990, 1, 1)
            };

            // Act
            var result = await _handler.UpdateCustomerAsync(request);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(request.Name, result.Data.Name);
            Assert.Equal(request.Email, result.Data.Email);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ExistingId_ReturnsSuccess()
        {
            // Arrange
            var customer = _context.Customers.First();
            var request = new Delete { Id = customer.Id };

            // Act
            var result = await _handler.DeleteCustomerAsync(request);

            // Assert
            Assert.Equal(200, result.StatusCode); 
            Assert.Equal("Customer deleted successfully.", result.Message);
            Assert.Null(_context.Customers.Find(customer.Id));
        }
    }
}
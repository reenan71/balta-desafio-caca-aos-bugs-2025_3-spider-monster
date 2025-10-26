using BugStore.Models;
using BugStore.Requests.Customers; 
using BugStore.Responses.Customers; 
using Create = BugStore.Requests.Customers.Create;
using Delete = BugStore.Requests.Customers.Delete;
using Get = BugStore.Requests.Customers.Get;
using GetById = BugStore.Requests.Customers.GetById;
using Update = BugStore.Requests.Customers.Update;

namespace BugStore.Test.UnitTests;

public class CustomerHandlerSimpleTests
{
    #region Customer Tests

    [Fact]
    public async Task Should_Create_Customer()
    {
        var request = new Create
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Phone = "1234567890"
        };

        var handler = new FakeCustomerHandler();
        var createdCustomer = await handler.CreateAsync(request, CancellationToken.None); // Substituído TestContext
        Assert.True(createdCustomer.Data?.Name == request.Name);
    }

    [Fact]
    public async Task Should_Update_Customer()
    {
        var handler = new FakeCustomerHandler();
        var createRequest = new Create
        {
            Name = "Initial Customer",
            Email = "initial@example.com",
            Phone = "0987654321"
        };
        var createdCustomer = await handler.CreateAsync(createRequest, CancellationToken.None);
        var updateRequest = new Update
        {
            Id = createdCustomer.Data!.Id,
            Name = "Updated Customer",
            Email = "updated@example.com",
            Phone = "1122334455"
        };
        var updatedCustomer = await handler.UpdateAsync(updateRequest, CancellationToken.None);
        Assert.True(updatedCustomer.Data?.Name == updateRequest.Name);
    }

    [Fact]
    public async Task Should_Delete_Customer()
    {
        var handler = new FakeCustomerHandler();
        var createRequest = new Create
        {
            Name = "Customer to Delete",
            Email = "delete@example.com",
            Phone = "5555555555"
        };
        var createdCustomer = await handler.CreateAsync(createRequest, CancellationToken.None);
        var deleteRequest = new Delete
        {
            Id = createdCustomer.Data!.Id
        };
        var deleteResult = await handler.DeleteAsync(deleteRequest, CancellationToken.None);
        Assert.True(deleteResult.IsSuccess);
    }

    [Fact]
    public async Task Should_Get_Customer_By_Id()
    {
        var handler = new FakeCustomerHandler();
        var createRequest = new Create
        {
            Name = "Customer to Retrieve",
            Email = "retrieve@example.com",
            Phone = "6666666666"
        };
        var createdCustomer = await handler.CreateAsync(createRequest, CancellationToken.None);
        var getByIdRequest = new GetById
        {
            Id = createdCustomer.Data!.Id
        };
        var retrievedCustomer = await handler.GetByIdAsync(getByIdRequest, CancellationToken.None);
        Assert.True(retrievedCustomer.Data?.Id == createdCustomer.Data.Id);
    }

    [Fact]
    public async Task Should_Get_Customers_List()
    {
        var handler = new FakeCustomerHandler();
        var createRequest = new Create
        {
            Name = "Customer 1",
            Email = "customer1@example.com",
            Phone = "7777777777"
        };

        await handler.CreateAsync(createRequest, CancellationToken.None);
        var getRequest = new Get
        {
            PageNumber = 1,
            PageSize = 10
        };

        var customersList = await handler.GetAsync(getRequest, CancellationToken.None);
        Assert.True(customersList.Data?.Count > 0);
    }

    #endregion
}

// Classe Fake para simulação
public class FakeCustomerHandler
{
    private List<Customer> _customers = new List<Customer>();

    public Task<Responses.Customers.Create> CreateAsync(Requests.Customers.Create request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone
        };
        _customers.Add(customer);
        return Task.FromResult(new Responses.Customers.Create(customer));
    }

    public Task<Responses.Customers.Update> UpdateAsync(Requests.Customers.Update request, CancellationToken cancellationToken)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == request.Id);
        if (customer != null)
        {
            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Phone = request.Phone;
        }
        return Task.FromResult(new Responses.Customers.Update(customer));
    }

    public Task<Responses.Customers.Delete> DeleteAsync(Requests.Customers.Delete request, CancellationToken cancellationToken)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == request.Id);
        if (customer != null)
        {
            _customers.Remove(customer);
            return Task.FromResult(new Responses.Customers.Delete(
                data: null, // Geralmente nulo em exclusões
                statusCode: 200, // Sucesso
                message: "Customer deleted"
            ));
        }
        return Task.FromResult(new Responses.Customers.Delete(
            data: null,
            statusCode: 404, // Não encontrado
            message: "Customer not found"
        ));
    }

    public Task<Responses.Customers.GetById> GetByIdAsync(Requests.Customers.GetById request, CancellationToken cancellationToken)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == request.Id);
        return Task.FromResult(new Responses.Customers.GetById(customer));
    }

    public Task<Responses.Customers.Get> GetAsync(Requests.Customers.Get request, CancellationToken cancellationToken)
    {
        var page = _customers
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return Task.FromResult(new Responses.Customers.Get(
            data: page,
            currentPage: request.PageNumber,
            pageSize: request.PageSize,
            totalCount: _customers.Count
        ));
    }
}

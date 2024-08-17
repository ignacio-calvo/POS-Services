using Microsoft.AspNetCore.Mvc;
using Moq;
using POS.Customers.API.Controllers;
using POS.Customers.Business.DTOs;
using POS.Customers.Business.Services.IServices.IServiceMappings;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace POS.Customers.API.Tests
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _mockService;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockService = new Mock<ICustomerService>();
            _controller = new CustomersController(_mockService.Object);
        }

        [Fact]
        public async Task GetCustomers_ReturnsOkResult_WithListOfCustomers()
        {
            // Arrange
            var customers = new List<CustomerDto>
            {
                new CustomerDto
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "123-456-7890"
                }
            };
            _mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCustomers = Assert.IsType<List<CustomerDto>>(okResult.Value);
            Assert.Single(returnCustomers);
        }

        [Fact]
        public async Task GetCustomer_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var customer = new CustomerDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890"
            };
            _mockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomer(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCustomer = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal(1, returnCustomer.Id);
        }

        [Fact]
        public async Task GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _controller.GetCustomer(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutCustomer_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var customer = new CustomerDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890"
            };
            _mockService.Setup(service => service.UpdateAsync(customer)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutCustomer(1, customer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostCustomer_ReturnsCreatedAtActionResult_WithCreatedCustomer()
        {
            // Arrange
            var customer = new CustomerDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890"
            };
            _mockService.Setup(service => service.AddAsync(customer)).ReturnsAsync(customer);

            // Act
            var result = await _controller.PostCustomer(customer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnCustomer = Assert.IsType<CustomerDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnCustomer.Id);
        }

        [Fact]
        public async Task PostCustomer_ReturnsConflict_WhenEmailAlreadyExists()
        {
            // Arrange
            var customer = new CustomerDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890"
            };
            _mockService.Setup(service => service.AddAsync(customer)).Throws(new UniqueConstraintViolationException("Email"));

            // Act
            var result = await _controller.PostCustomer(customer);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal("A customer with the same email already exists.", ((dynamic)conflictResult.Value).message);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var customer = new CustomerDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890"
            };
            _mockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(customer);
            _mockService.Setup(service => service.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }

    // Custom exception class for unique constraint violations
    public class UniqueConstraintViolationException : System.Exception
    {
        public UniqueConstraintViolationException(string message) : base(message) { }
    }
}

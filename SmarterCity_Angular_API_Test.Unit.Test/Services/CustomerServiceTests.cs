using SmarterCity_Angular_API_Test.Persistence;
using SmarterCity_Angular_API_Test.Services;
using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using AutoFixture;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace SmarterCity_Angular_API_Test.Unit.Test.Services
{
    public class CustomerServiceTests
    {
        public Mock<ICustomerRepository> mockCustomerRepository = new Mock<ICustomerRepository>();

        private ICustomerService InitialiseInMemoryRepository(List<Customer> customers = null)
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions { CompactionPercentage = 1 } );
            memoryCache.Set(CustomerRepository.MemoryCacheKey, customers);
            return new CustomerService(new CustomerRepository(memoryCache));
        }

        [Fact]
        public async Task GetCustomers_EmptyRepository_ReturnsNoCustomersAsync()
        {
            IEnumerable<Customer> expected = new List<Customer>();
            
            mockCustomerRepository.Setup(p => p.Get()).Returns(Task.FromResult(expected));

            var customerService = new CustomerService(mockCustomerRepository.Object);
            var actual = await customerService.Get();
            
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Count());
        }

        [Fact]
        public async Task GetCustomers_CustomersExistInRepository_Returns1OrMoreCustomersAsync()
        {
            Fixture fixture = new Fixture();
            var expected = fixture.Create<IEnumerable<Customer>>();
            
            mockCustomerRepository.Setup(p => p.Get()).Returns(Task.FromResult(expected));

            var customerService = new CustomerService(mockCustomerRepository.Object);
            var actual = await customerService.Get();

            Assert.NotNull(actual);
            Assert.Equal(expected.Last().Mobilenumber, actual.Last().Mobilenumber);
            Assert.Equal(expected.Count(), actual.Count());
        }

        [Fact]
        public async Task GetCustomerById_ValidId_ReturnsCustomerAsync()
        {
            var id = 1;
            Fixture fixture = new Fixture();
            var expected = fixture.CreateMany<Customer>().ToList();
            expected.First().Id = id;

            var customerService = InitialiseInMemoryRepository(expected);
            var actual =await customerService.GetById(id);

            Assert.NotNull(actual);
            Assert.Equal(expected.First().Id, actual.Id);
        }

        [Fact]
        public async Task GetCustomerById_InValidId_ReturnsNoCustomerAsync()
        {
            var id = 1;
            Fixture fixture = new Fixture();
            var expected = fixture.CreateMany<Customer>().ToList();
            expected.First().Id = id*10;

            var customerService = InitialiseInMemoryRepository(expected);
            var actual = await customerService.GetById(id);

            Assert.Null(actual);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCustomerAsync()
        {
            Fixture fixture = new Fixture();
            var expected = fixture.Create<Customer>();

            var customerService = InitialiseInMemoryRepository();
            var actual = await customerService.Create(expected);

            Assert.NotNull(actual);
            Assert.NotEqual(0, actual.Id);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.Mobilenumber, actual.Mobilenumber);
        }
    }
}

using SmarterCity_Angular_API_Test.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterCity_Angular_API_Test.Services
{
    public class CustomerService: ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<IEnumerable<Customer>> Get() =>
             _customerRepository.Get();

        public Task<Customer> GetById(int id) =>
             _customerRepository.GetById(id);

        public Task<Customer> Create(Customer customer) =>
             _customerRepository.Create(customer);
    }
}

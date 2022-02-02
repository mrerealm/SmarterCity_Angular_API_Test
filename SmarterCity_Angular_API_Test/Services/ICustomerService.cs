using SmarterCity_Angular_API_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterCity_Angular_API_Test.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> Get();
        Task<Customer> GetById(int id);
        Task<Customer> Create(Customer customer);
    }
}

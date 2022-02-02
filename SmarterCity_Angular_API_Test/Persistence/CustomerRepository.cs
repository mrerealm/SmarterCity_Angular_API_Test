using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterCity_Angular_API_Test.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;
        private readonly IMemoryCache _memoryCache;
        public static string MemoryCacheKey { get; } = "InMemoryCustomerRepository";

        public CustomerRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                Priority = CacheItemPriority.Normal
            };

            object value;
            if (!_memoryCache.TryGetValue(MemoryCacheKey, out value))
                _memoryCache.Set(MemoryCacheKey, new List<Customer>(), _cacheEntryOptions);
        }

        public Task<IEnumerable<Customer>> Get()
        {
            IEnumerable<Customer> result = new List<Customer>();

            object value;
            if (_memoryCache is not null && _memoryCache.TryGetValue(MemoryCacheKey, out value))
                result = (List<Customer>)value;

            return Task.FromResult(result);
        }

        public Task<Customer> GetById(int id)
        {
            Customer result = null;

            object value;
            if (_memoryCache is not null && _memoryCache.TryGetValue(MemoryCacheKey, out value))
            {
                var list = (List<Customer>)value;
                if (list is not null)
                    result = list.FirstOrDefault(x => x.Id == id);
            }
            return Task.FromResult(result);

        }

        public Task<Customer> Create(Customer customer)
        {
            Customer result = null;
            var list = new List<Customer>();
            object value;
            if (_memoryCache.TryGetValue(MemoryCacheKey, out value) && value is not null)
            {
                list = (List<Customer>)value;
                var last = list.LastOrDefault();
                if (last is not null)
                    customer.Id = last.Id + 1;
                else
                    customer.Id = 1;
            }

            list.Add(customer);
            _memoryCache.Set(MemoryCacheKey, list, _cacheEntryOptions);
            result = customer;

            return Task.FromResult(result);
        }
    }
}

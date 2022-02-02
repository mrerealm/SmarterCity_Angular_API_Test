using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SmarterCity_Angular_API_Test.Persistence;
using SmarterCity_Angular_API_Test.Services;

namespace SmarterCity_Angular_API_Test.Unit.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IMemoryCache, MemoryCache>();
        }
    }
}

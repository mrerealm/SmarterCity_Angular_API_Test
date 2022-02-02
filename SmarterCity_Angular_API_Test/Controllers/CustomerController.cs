using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmarterCity_Angular_API_Test.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterCity_Angular_API_Test.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger,
            ICustomerService customerService)
        {
            _logger = logger;
            _customerService =  customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok( await _customerService.Get());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message, ex);
                return BadRequest(ex.GetBaseException().Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _customerService.GetById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message, ex);
                return BadRequest(ex.GetBaseException().Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            try
            {
                return Ok(await _customerService.Create(customer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message, ex);
                return BadRequest(ex.GetBaseException().Message);
            }
        }
    }
}

using CustomerManager.Application.Services.CustomerLogic;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManager.Api.Controllers
{
    [Route("api/customers")]

    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            return Ok(await customerService.GetCustomer(id));
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await customerService.GetCustomers());
        }
    }
}

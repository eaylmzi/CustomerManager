using Azure;
using CustomerManager.Application.Dtos.Customers;
using CustomerManager.Application.Services.CustomerLogic;
using CustomerManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using CustomerManager.Application.Common;



namespace CustomerManager.Api.Controllers
{
    [Route("api/customers")]

    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> AddCustomer([FromBody] CustomerDto customerDto)
        {

            ApiResponse<int> response = await customerService.AddCustomer(customerDto);
            if(response.Data == 0)
            {
                return StatusCode(500, response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCustomer(int id)
        {
            ApiResponse<bool> response = await customerService.DeleteCustomer(id);
            if (response.Data == false)
            {
                return StatusCode(500, response);
            }
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomer(int id)
        {
            ApiResponse<CustomerDto> response = await customerService.GetCustomer(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
        {
            ApiResponse<List<CustomerDto>> response = await customerService.GetCustomers();
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            ApiResponse<CustomerDto> response = await customerService.UpdateCustomer(id, customerDto);
            if (response.Data == null)
            {
                return StatusCode(500, response);
            }      
            return Ok(response); 
        }
      

    }
}

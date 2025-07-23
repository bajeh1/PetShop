using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShop.Application.AppResponses;
using PetShop.Application.Commands;
using PetShop.Application.Dtos;
using PetShop.Application.Queries.Customers;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(IMapper mapper, IMediator mediator, ILogger<CustomerController> logger ) : ControllerBase
    {
        // GET: api/<CustomerController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllCustomersQuery();
            var response = await mediator.Send(query);
            return Ok(response);
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
               var query = new GetCustomerByIdQuery { Id = Guid.Parse(id) };
                var response = await mediator.Send(query);
                if (response.Success) return Ok(response);
                return BadRequest(response);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while processing your request");
                return StatusCode(500, new CreateCustomerResponse(false, "Server error occurred. Could not Process the request "));
            }
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]  CreateCustomerDto customerDto)
        {
            try
            {
                var command = mapper.Map<CreateCustomerCommand>(customerDto);
                var response = await mediator.Send(command);
                if (response.Success) return Ok(response);
                return BadRequest(response);
            }
            catch (Exception e)
            {
               logger.LogError(e, "An error occurred while processing your request");
                return StatusCode(500, new CreateCustomerResponse(false, "Server error occurred. Could not Process the request "));
            }
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

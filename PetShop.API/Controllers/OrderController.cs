using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShop.Application.AppResponses;
using PetShop.Application.Commands;
using PetShop.Application.Commands.Orders;
using PetShop.Application.Dtos;
using PetShop.Application.Queries.Customers;
using PetShop.Application.Queries.Orders;

namespace PetShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IMapper mapper, IMediator mediator, ILogger<OrderController> logger ) : ControllerBase
    {
        // GET: api/<OrderController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllOrdersQuery();
            var response = await mediator.Send(query);
            return Ok(response);
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
               var query = new GetOrderByIdQuery() { Id = Guid.Parse(id) };
                var response = await mediator.Send(query);
                if (response.Success) return Ok(response);
                return BadRequest(response);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while processing your request");
                return StatusCode(500, new GetSingleOrdersResponse(false, "Server error occurred. Could not Process the request ", null));
            }
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]  CreateOrderDto dto)
        {
                var command = mapper.Map<CreateOrderCommand>(dto);
                var response = await mediator.Send(command);
                if (response.Success) return Ok(response);
                return BadRequest(response);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public  async Task<IActionResult> Put(string id, [FromBody] UpdateOrderDto dto)
        {
            var command = mapper.Map<UpdateOrderCommand>(dto);
            command.Id = Guid.Parse(id);
            var response = await mediator.Send(command);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }
        
        [HttpPut, Route("OrderItems")]
        public  async Task<IActionResult> UpdateOrderItems([FromBody] UpdateOrderItemsDto dto)
        {
            var command = mapper.Map<UpdateOrderItemsCommand>(dto);
            var response = await mediator.Send(command);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

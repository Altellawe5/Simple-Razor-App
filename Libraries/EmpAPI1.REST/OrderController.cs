using AutoMapper;
using Menu.Domain.Interfaces;
using Menu.Domain.Models;
using Menu.Infrastructure.Repositories;
using Menu.REST.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.REST
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        [HttpGet("{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetActiveOrders(Guid customerId)
        {
            var orderDTOs = await _orderRepository.GetActiveOrdersByCustomerId(customerId);

            if (orderDTOs == null)
            {
                return NotFound();
            }

            return Ok(orderDTOs);
        }
        [HttpGet("Unpaid/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersPerCustomerIdNotPayed(Guid customerId)
        {
            var orders = await _orderRepository.GetUnpaidOrdersByCustomerId(customerId);
            if (orders == null)
            {
                return NotFound();
            }

            var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            return Ok(orderDTOs);
        }

        [HttpPost("Pay/{orderId}")]
        public async Task<IActionResult> PayOrder(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.PaymentDate = DateTime.UtcNow;
            await _orderRepository.UpdateOrder(order);

            return NoContent();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);  
            order.Id = Guid.NewGuid();
            order.PaymentDate = null;
            order.CreationDate = DateTime.Now;
            await _orderRepository.CreateOrder(order);
            return Ok(order);
        }
        [HttpGet("GetOrder/{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(Guid id)
        {
            var order = await _orderRepository.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Order>(order));
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteOrder(order);

            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDTO>> UpdateOrder(Guid id, OrderDTO orderDto)
        {
            if (id != orderDto.Id)
            {
                return BadRequest();
            }
            await _orderRepository.UpdateOrder(_mapper.Map<Order>(orderDto));
            return NoContent();
        }


    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Menu.Domain.Interfaces;
using Menu.Domain.Models;
using Menu.REST.DTO;
using Menu.Infrastructure.DTO;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<CustomerDTO>>(customers));
    }
    [HttpGet("ByName/{firstName}/{lastName}")]
    public async Task<ActionResult<Customer>> GetCustomerByName(string firstName, string lastName)
    {
        var customer = await _customerRepository.GetByNameAsync(firstName, lastName);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDTO>> GetCustomer(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CustomerDTO>(customer));
    }

    // POST: api/Customers
    [HttpPost]
    public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO customerDTO)
    {
        var customer = _mapper.Map<Customer>(customerDTO);
        customer.Password = BCrypt.Net.BCrypt.HashPassword(customerDTO.Password);
        customer.Id = new Guid();
        customer.IsActive = true;
        await _customerRepository.AddAsync(customer);

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, _mapper.Map<CustomerDTO>(customer));
    }

    // PUT: api/Customers/5
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDTO>> PutCustomer(Guid id, CustomerDTO customerDTO)
    {
        if (id != customerDTO.Id)
        {
            return BadRequest();
        }
        customerDTO.Password = BCrypt.Net.BCrypt.HashPassword(customerDTO.Password);
        await _customerRepository.UpdateAsync(_mapper.Map<Customer>(customerDTO));     

        return NoContent();
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            return NotFound();
        }
        await _customerRepository.DeleteAsync(customer.Id);


        return NoContent();
    }
}

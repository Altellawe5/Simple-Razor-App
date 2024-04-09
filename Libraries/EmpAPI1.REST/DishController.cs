using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Menu.Domain.Models;
using Menu.Domain.Interfaces;
using Menu.REST.DTO;
using System.Net.Mime;

[Route("api/[controller]")]
[ApiController]
#if ProducesConsumes
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
#endif
public class DishController : ControllerBase
{
    private readonly IDishRepository _dishRepository;
    private readonly IMapper _mapper;

    public DishController(IDishRepository dishRepository, IMapper mapper)
    {
        _dishRepository = dishRepository;
        _mapper = mapper;
    }


    // GET: api/menu
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDTO>>> GetAllDishes()
    {
        var dishes = await _dishRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<DishDTO>>(dishes));
    }

    // GET: api/menu/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<DishDTO>> GetDish(Guid id)
    {
        var dish = await _dishRepository.GetByIdAsync(id);
        if (dish == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<DishDTO>(dish));
    }

    // POST: api/menu
    [HttpPost]
    public async Task<ActionResult<DishDTO>> CreateDish(DishDTO dishDto)
    {
        var dish = _mapper.Map<Dish>(dishDto);
        dish.Id = new Guid();
        await _dishRepository.AddAsync(dish);

        return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, _mapper.Map<DishDTO>(dish));
    }

    // PUT: api/menu/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDish(Guid id, [FromBody] DishDTO dishDto)
    {
        if (id != dishDto.Id)
        {
            return BadRequest();
        }
     

        await _dishRepository.UpdateAsync(_mapper.Map<Dish>(dishDto));
        return NoContent();
    }

    // DELETE: api/menu/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDish(Guid id)
    {
        var dish = await _dishRepository.GetByIdAsync(id);
        if (dish == null)
        {
            return NotFound();
        }

        await _dishRepository.DeleteAsync(dish);
        return NoContent();
    }
}

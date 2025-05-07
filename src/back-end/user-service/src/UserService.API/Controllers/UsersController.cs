using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;

    public UsersController(IUserRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _repo.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        await _repo.AddAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, User user)
    {
        user.Id = id;
        await _repo.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}

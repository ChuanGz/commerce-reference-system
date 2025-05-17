using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserRepository repo) : ControllerBase
{
    private readonly IUserRepository _repo = repo;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _repo.AddAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        user.Id = id;
        await _repo.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
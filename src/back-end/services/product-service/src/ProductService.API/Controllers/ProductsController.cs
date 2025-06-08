using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Commands;
using ProductService.Application.Queries;
using ProductService.Domain.Constants;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetActiveProductsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken = default
        )
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("sku/{sku}")]
        public async Task<IActionResult> GetBySKU(
            string sku,
            CancellationToken cancellationToken = default
        )
        {
            var result = await _mediator.Send(new GetProductBySKUQuery(sku), cancellationToken);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(
            string category,
            CancellationToken cancellationToken = default
        )
        {
            var result = await _mediator.Send(
                new GetProductsByCategoryQuery(category),
                cancellationToken
            );
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCommand command,
            CancellationToken cancellationToken = default
        )
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateProductCommand command,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            if (id != command.Id)
                return BadRequest(ErrorMessages.IdMismatch);

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken = default
        )
        {
            await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return NoContent();
        }
    }
}

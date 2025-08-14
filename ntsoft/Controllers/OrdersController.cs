using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ntsoft.Data;
using ntsoft.Model;

namespace ntsoft.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(ApplicationDbContext db) : ControllerBase
{
    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateRequest dto)
    {
        var sub = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(sub)) return Unauthorized();

        var product = await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == dto.ProductId);
        if (product is null) return BadRequest("Product not found.");

        var order = new Order
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            CustomerUserId = sub
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [Authorize(Roles = "Customer")]
    [HttpGet("mine")]
    public async Task<IActionResult> Mine()
    {
        var sub = User.FindFirst("sub")?.Value;
        var list = await db.Orders
            .Where(o => o.CustomerUserId == sub)
            .OrderByDescending(o => o.Id)
            .ToListAsync();

        return Ok(list);
    }

    [Authorize(Roles = "Customer,Admin")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await db.Orders.FindAsync(id);
        if (order is null) return NotFound();

        var sub = User.FindFirst("sub")?.Value;
        if (!User.IsInRole("Admin") && order.CustomerUserId != sub) return Forbid();

        return Ok(order);
    }
}

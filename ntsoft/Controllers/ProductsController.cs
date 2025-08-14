using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ntsoft.Data;
using ntsoft.Model;

namespace ntsoft.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await db.Products.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return p is null ? NotFound() : Ok(p);
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateRequest dto)
    {
        var sub = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(sub)) return Unauthorized();

        var p = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            OwnerUserId = sub
        };

        db.Products.Add(p);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateRequest dto)
    {
        var sub = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(sub)) return Unauthorized();

        var p = await db.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return NotFound();
        if (!User.IsInRole("Admin") && p.OwnerUserId != sub) return Forbid();

        p.Name = dto.Name;
        p.Price = dto.Price;
        await db.SaveChangesAsync();
        return Ok(p);
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var sub = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(sub)) return Unauthorized();

        var p = await db.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return NotFound();
        if (!User.IsInRole("Admin") && p.OwnerUserId != sub) return Forbid();

        db.Products.Remove(p);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

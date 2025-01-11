using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly AuthDbContext _context;

    public ProductController(AuthDbContext context)
    {
        _context = context;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        try
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Size)
                .ToListAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("page")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsAtPage([FromQuery] int page = 1)
    {
        try
        {
            int pageSize = 9;
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Size)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Get total count for pagination info
            var totalProducts = await _context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var response = new
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = totalProducts
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }
}
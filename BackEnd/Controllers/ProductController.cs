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

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(
        [FromQuery] string? name,
        [FromQuery] int? category,
        [FromQuery] int? fromStar,
        [FromQuery] int? toStar,
        [FromQuery] decimal? fromPrice,
        [FromQuery] decimal? toPrice)
    {
        try
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Size)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (category.HasValue)
                query = query.Where(p => p.CategoryId == category);

            if (fromStar.HasValue)
                query = query.Where(p => p.AvbRating >= fromStar);

            if (toStar.HasValue)
                query = query.Where(p => p.AvbRating <= toStar);

            if (fromPrice.HasValue)
                query = query.Where(p => p.ProductDetails.Any(pd => pd.Price >= fromPrice));

            if (toPrice.HasValue)
                query = query.Where(p => p.ProductDetails.Any(pd => pd.Price <= toPrice));

            var products = await query.ToListAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("detail")]
    public async Task<ActionResult<ProductWithDetails>> GetProductDetail([FromQuery] int id)
    {
        try
        {
            var productWithDetails = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductDetails)
                    .ThenInclude(pd => pd.Size)
                .Where(p => p.Id == id)
                .Select(p => new ProductWithDetails
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                    AvbRating = p.AvbRating,
                    Status = p.Status,
                    CategoryName = p.Category.Name,
                    Details = p.ProductDetails.Select(pd => new ProductDetailWithSize
                    {
                        Id = pd.Id,
                        SizeId = pd.SizeId,
                        SizeName = pd.Size.size_Name,
                        Quantity = pd.Quantity,
                        Price = pd.Price,
                        Status = pd.Status
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (productWithDetails == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            return Ok(productWithDetails);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
using System.Collections.Generic;

public class ProductWithDetails
{
    // Product properties
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public double AvbRating { get; set; }
    public int Status { get; set; }
    public string CategoryName { get; set; }

    // List of product details with size info
    public List<ProductDetailWithSize> Details { get; set; }
}
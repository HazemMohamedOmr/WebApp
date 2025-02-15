namespace WebApp.Core.Models;

public class Product : BaseEntity<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }

    // Relationships

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}
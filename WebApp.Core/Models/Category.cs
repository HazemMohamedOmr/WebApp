namespace WebApp.Core.Models;

public class Category : BaseEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Product> Products { get; set; }
}
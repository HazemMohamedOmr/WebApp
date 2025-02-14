namespace WebApp.Core.Models;

public class Supplier : BaseEntity<int>
{
    public string CompanyName { get; set; }
    public string ContactInfo { get; set; }
    public ICollection<Product> Products { get; set; }
}
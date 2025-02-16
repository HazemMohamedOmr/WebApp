using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Data.Data;

namespace WebApp.Data.Repositories;

public class ProductRepository : Repository<Product, int>, IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<Product> DoSomethingSpecialToThisClass()
    {
        throw new NotImplementedException();
    }
}
using WebApp.Core.Models;

namespace WebApp.Core.Repositories;

public interface IProductRepository : IRepository<Product, int>
{
    Task<Product> DoSomethingSpecialToThisClass();
}
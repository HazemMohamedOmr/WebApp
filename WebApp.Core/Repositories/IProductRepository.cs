using WebApp.Core.Models;

namespace WebApp.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> DoSomethingSpecialToThisClass();
    }
}
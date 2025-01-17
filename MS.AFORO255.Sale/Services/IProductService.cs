using System.Threading.Tasks;

namespace MS.AFORO255.Sale.Services
{
    public interface IProductService
    {
        Task<bool> ValidateProduct(int product_id);
    }
}

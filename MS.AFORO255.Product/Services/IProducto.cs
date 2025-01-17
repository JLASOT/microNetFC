using MS.AFORO255.Product.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MS.AFORO255.Product.Services
{
    public interface IProducto
    {
        // Obtener todos los productos
        IEnumerable<Producto> GetAll();

        // Obtener un producto por su ID
        Producto GetById(int id);

        // Agregar un nuevo producto
        void Add(Producto producto);

        // Actualizar un producto existente
        void Update(Producto producto);

        // Eliminar un producto por su ID
        void Delete(int id);


        // Métodos CRUD para detalles de ventas
        Task<List<Categoria>> GetAllCategorias();
        void AddCategoria(Categoria categoria);
    }
}

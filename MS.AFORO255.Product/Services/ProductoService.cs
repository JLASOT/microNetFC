using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MS.AFORO255.Product.Models;
using MS.AFORO255.Product.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS.AFORO255.Product.Services
{
    public class ProductoService : IProducto
    {
        private readonly ContextDatabase _contextDatabase;

        public ProductoService(ContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        // Obtener todos los productos, incluyendo la categoría asociada
        public IEnumerable<Producto> GetAll()
        {
            return _contextDatabase.Producto.Include(x => x.Categoria)  // Incluir la categoría asociada
                .AsNoTracking()  // Evitar el seguimiento de cambios si no es necesario
                .ToList();
        }

        // Obtener un producto por su ID
        public Producto GetById(int id)
        {
            return _contextDatabase.Producto.Include(x => x.Categoria)
                .FirstOrDefault(p => p.IdProducto == id);
        }

        // Crear un nuevo producto
        public void Create(Producto producto)
        {
            _contextDatabase.Producto.Add(producto);
            _contextDatabase.SaveChanges();
        }

        // Actualizar un producto existente
        public void Update(Producto producto)
        {
            _contextDatabase.Producto.Update(producto);
            _contextDatabase.SaveChanges();
        }

        // Eliminar un producto
        public void Delete(int id)
        {
            var producto = _contextDatabase.Producto.Find(id);
            if (producto != null)
            {
                _contextDatabase.Producto.Remove(producto);
                _contextDatabase.SaveChanges();
            }
        }

        public void Add(Producto producto)
        {
            _contextDatabase.Producto.Add(producto);
            _contextDatabase.SaveChanges();
        }

        public void AddCategoria(Categoria categoria)
        {
            _contextDatabase.Add(categoria);
            _contextDatabase.SaveChanges(); // Guarda los cambios en la base de datos
        }



        /*
         public async Task<IEnumerable<Categoria>> GetAllCategoria()
        {
            return await _contextDatabase.Categoria.ToListAsync();
        }

        public async Task<Categoria> GetCategoriaById(int id)
        {
            return await _contextDatabase.Categoria
                .FirstOrDefaultAsync(c => c.IdCategoria == id);
        }
         */

        public void UpdateCategoria(Categoria categoria)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategoria(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Producto>> ValidateProducts(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public Task<Producto> ValidateProductById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Categoria>> GetAllCategorias()
        {
            return await _contextDatabase.Categoria.ToListAsync();
        }
    }
}

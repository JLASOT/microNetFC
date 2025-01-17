using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.Product.Models;
using MS.AFORO255.Product.Services;
using System.Threading.Tasks;

namespace MS.AFORO255.Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProducto _productoService;

        
        public ProductoController(IProducto productoService)
        {
            _productoService = productoService;
        }

        // Obtener todos los productos
        [HttpGet]
        public IActionResult Get()
        {
            var productos = _productoService.GetAll();
            return Ok(productos);
        }

        // Obtener un producto por su ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var producto = _productoService.GetById(id);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }
            return Ok(producto);
        }

        // Agregar un nuevo producto
        [HttpPost]
        public IActionResult Add([FromBody] Producto producto)
        {
            if (producto == null)
            {
                return BadRequest(new { message = "Datos inválidos" });
            }

            _productoService.Add(producto); // El servicio ya maneja la creación sin necesidad de devolver un valor
            return CreatedAtAction(nameof(GetById), new { id = producto.IdProducto }, producto);
        }

        // Actualizar un producto existente
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return BadRequest(new { message = "El ID del producto no coincide" });
            }

            var existingProducto = _productoService.GetById(id);
            if (existingProducto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            _productoService.Update(producto); // El servicio ya maneja la actualización sin necesidad de devolver un valor
            return Ok(producto);
        }

        // Eliminar un producto por su ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var producto = _productoService.GetById(id);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            _productoService.Delete(id); // El servicio ya maneja la eliminación sin necesidad de devolver un valor
            return NoContent();
        }

        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _productoService.GetAllCategorias();
            if (categorias == null || categorias.Count == 0)
            {
                return NotFound(new { message = "No se encontraron categorías" });
            }
            return Ok(categorias);
        }

        [HttpPost("categorias")]
        public IActionResult AddCategory([FromBody] Categoria categoria)
        {
            if (categoria == null || string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                return BadRequest(new { message = "Los datos de la categoría son inválidos." });
            }
            _productoService.AddCategoria(categoria); // El servicio maneja la creación
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoria.IdCategoria }, categoria);
        }
        public IActionResult GetCategoryById(int id)
        {
            var categoria = _productoService.GetById(id);
            if (categoria == null)
            {
                return NotFound(new { message = "Categoria no encontrado" });
            }
            return Ok(categoria);
        }
    }
}

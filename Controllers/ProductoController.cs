using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ProductRepo;
namespace MiWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductoController : ControllerBase
{
    private readonly ProductoRepository _productoRepository;
    private readonly ProductValidator _productValidator;
    public ProductoController(ProductoRepository productoRepository, ProductValidator productValidator)
    {
        _productoRepository = productoRepository; // Usa la instancia inyectada
        _productValidator = productValidator;
    }
    [HttpGet]
    public ActionResult getProducts()
    {
        var res = _productoRepository.GetAll();
        return Ok(res);
    }

    // [HttpGet("getProductsById/{id}")]
    // public ActionResult getProductsPorId(int id)
    // {
    //     var res = _productoRepository.GetById(id);
    //     return Ok(res);
    // }

    [HttpPost]
    public ActionResult createProduct([FromBody] Product newProduct)
    {
        ValidationResult verification = _productValidator.Validate(newProduct);
        if (!verification.IsValid) return NotFound(new { message = "Producto invalido" });
        var res = _productoRepository.Create(newProduct);
        return Ok(new { message = "Producto creado", newProduct = res });
    }

    [HttpPut("{id}")]
    public ActionResult updateProduct([FromBody] Product newProduct, int id)
    {
        ValidationResult verification = _productValidator.Validate(newProduct);
        if (!verification.IsValid) return NotFound(new { message = "Producto invalido" });
        var res = _productoRepository.Update(newProduct, id);
        if(res == null) return NotFound(new { message = "Producto invalido" });
        return Ok(new { message = "Producto actualizado", newProduct = res });
    }

    // [HttpDelete("deleteProduct/{id}")]
    // public ActionResult deleteProduct(int id)
    // {
    //     var res = _productoRepository.Remove(id);
    //     if (!res) return NotFound(new { message = "Producto invalido" });
    //     return Ok(new { message = "Producto borrado" });
    // }


}


using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ProductRepo;
namespace MiWebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class ProductController : ControllerBase
{
    private readonly ProductRepository _productRepository;
    private readonly ProductValidator _productValidator;
    public ProductController(ProductRepository productoRepository, ProductValidator productValidator)
    {
        _productRepository = productoRepository; // Usa la instancia inyectada
        _productValidator = productValidator;
    }
    [HttpGet("/getProducts")]
    public ActionResult getProducts()
    {
        var res = _productRepository.GetAll();
        return Ok(res);
    }

    [HttpGet("/getProductsById/{id}")]
    public ActionResult getProductsPorId(int id)
    {
        var res = _productRepository.GetById(id);
        return Ok(res);
    }

    [HttpPost("/createProduct")]
    public ActionResult createProduct([FromBody] Product newProduct)
    {
        ValidationResult verification = _productValidator.Validate(newProduct);
        if (!verification.IsValid) return NotFound(new { message = "Producto invalido" });
        var res = _productRepository.Create(newProduct);
        return Ok(new { message = "Producto creado", newProduct = res });
    }

    [HttpPut("/updateProduct/{id}")]
    public ActionResult updateProduct([FromBody] Product newProduct, int id)
    {
        ValidationResult verification = _productValidator.Validate(newProduct);
        if (!verification.IsValid) return NotFound(new { message = "Producto invalido" });
        var res = _productRepository.Update(newProduct, id);
        if(res == null) return NotFound(new { message = "Producto invalido" });
        return Ok(new { message = "Producto actualizado", newProduct = res });
    }

    [HttpDelete("/deleteProduct/{id}")]
    public ActionResult deleteProduct(int id)
    {
        var res = _productRepository.Remove(id);
        if (!res) return NotFound(new { message = "Producto invalido" });
        return Ok(new { message = "Producto borrado" });
    }


}


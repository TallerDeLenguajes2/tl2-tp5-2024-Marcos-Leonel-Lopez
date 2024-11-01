using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace MiWebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class ProductController : ControllerBase
{
    private readonly ProductRepository _productoRepository;
    private readonly ProductValidator _productValidator;
    public ProductController(ProductRepository productoRepository, ProductValidator productValidator)
    {
        _productoRepository = productoRepository; // Usa la instancia inyectada
        _productValidator = productValidator;
    }
    [HttpGet("/getProducts")]
    public ActionResult getProducts()
    {
        var res = _productoRepository.GetAll();
        return Ok(res);
    }

    [HttpGet("/getProductsById/{id}")]
    public ActionResult getProductsPorId(int id)
    {
        var res = _productoRepository.GetById(id);
        return Ok(res);
    }

    [HttpPost("/createProduct")]
    public ActionResult createProduct([FromBody] Product newProduct)
    {
        ValidationResult verification = _productValidator.Validate(newProduct);
        if(!verification.IsValid) return NotFound(new {message="Producto invalido"});
            var res = _productoRepository.Create(newProduct);
        return  Ok(new {message="Producto creado",newProduct});
    }


}


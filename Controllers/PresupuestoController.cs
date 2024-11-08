using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PresupuestoRepo;
using ProductRepo;
namespace MiWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PresupuestoController : ControllerBase
{
    private readonly PresupuestoRepository _presupuestoRepository;

    public PresupuestoController(PresupuestoRepository presupuestoRepository)
    {
        _presupuestoRepository = presupuestoRepository;
    }

    [HttpGet]
    public ActionResult getPresupuesto()
    {
        var res = _presupuestoRepository.GetAll();
        return Ok(res);
    }

    [HttpGet("{id}")]
    public ActionResult getProductsPorId(int id)
    {
        var res = _presupuestoRepository.GetById(id);
        return Ok(res);
    }

    [HttpPost]

    public ActionResult postPresupuesto([FromBody] Presupuesto presupuesto)
    {
        var res = _presupuestoRepository.Create(presupuesto);
        return Ok(res);
    }

    [HttpPost("{id}/ProductoDetalle")]
    public ActionResult addProductToDetail([FromBody] PresupuestosDetalle nuevoDetalle, int id)
    {
        var res = _presupuestoRepository.Update(nuevoDetalle, id);
        return Ok(res);
    }
}
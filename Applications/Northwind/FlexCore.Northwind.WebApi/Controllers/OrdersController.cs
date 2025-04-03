using Microsoft.AspNetCore.Mvc;
using FlexCore.Northwind;

namespace FlexCore.Northwind.Api.Controllers;

/// <summary>
/// Controller per gestire le operazioni CRUD sugli ordini.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly NorthwindService _northwindService;

    public OrdersController(NorthwindService northwindService)
    {
        _northwindService = northwindService;
    }

    /// <summary>
    /// Ottiene tutti gli ordini.
    /// </summary>
    /// <returns>Lista di ordini.</returns>
    [HttpGet]
    public async Task<IEnumerable<Order>> GetAll()
    {
        return await _northwindService.GetAllOrdersAsync();
    }

    /// <summary>
    /// Aggiunge un nuovo ordine.
    /// </summary>
    /// <param name="order">Ordine da aggiungere.</param>
    /// <returns>Task di completamento.</returns>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Order order)
    {
        await _northwindService.AddOrderAsync(order);
        return CreatedAtAction(nameof(GetAll), new { id = order.Id }, order);
    }

    /// <summary>
    /// Modifica un ordine esistente.
    /// </summary>
    /// <param name="id">ID dell'ordine da modificare.</param>
    /// <param name="order">Ordine da modificare.</param>
    /// <returns>Task di completamento.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Order order)
    {
        order.Id = id;
        await _northwindService.UpdateOrderAsync(order);
        return NoContent();
    }

    /// <summary>
    /// Elimina un ordine.
    /// </summary>
    /// <param name="id">ID dell'ordine da eliminare.</param>
    /// <returns>Task di completamento.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _northwindService.DeleteOrderAsync(id);
        return NoContent();
    }
}

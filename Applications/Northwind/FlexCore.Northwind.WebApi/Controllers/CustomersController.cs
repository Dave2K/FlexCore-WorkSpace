using Microsoft.AspNetCore.Mvc;
using FlexCore.Northwind;

namespace FlexCore.Northwind.Api.Controllers;

/// <summary>
/// Controller per gestire le operazioni CRUD sui clienti.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly NorthwindService _northwindService;

    public CustomersController(NorthwindService northwindService)
    {
        _northwindService = northwindService;
    }

    /// <summary>
    /// Ottiene tutti i clienti.
    /// </summary>
    /// <returns>Lista di clienti.</returns>
    [HttpGet]
    public async Task<IEnumerable<Customer>> GetAll()
    {
        return await _northwindService.GetAllCustomersAsync();
    }

    /// <summary>
    /// Aggiunge un nuovo cliente.
    /// </summary>
    /// <param name="customer">Cliente da aggiungere.</param>
    /// <returns>Task di completamento.</returns>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Customer customer)
    {
        await _northwindService.AddCustomerAsync(customer);
        return CreatedAtAction(nameof(GetAll), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Modifica un cliente esistente.
    /// </summary>
    /// <param name="id">ID del cliente da modificare.</param>
    /// <param name="customer">Cliente da modificare.</param>
    /// <returns>Task di completamento.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
    {
        customer.Id = id;
        await _northwindService.UpdateCustomerAsync(customer);
        return NoContent();
    }

    /// <summary>
    /// Elimina un cliente.
    /// </summary>
    /// <param name="id">ID del cliente da eliminare.</param>
    /// <returns>Task di completamento.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _northwindService.DeleteCustomerAsync(id);
        return NoContent();
    }
}

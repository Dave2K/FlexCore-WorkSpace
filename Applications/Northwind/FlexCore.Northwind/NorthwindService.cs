namespace FlexCore.Northwind;

/// <summary>
/// Servizio per gestire le operazioni CRUD per le entità di Northwind.
/// </summary>
public class NorthwindService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Order> _orderRepository;

    /// <summary>
    /// Costruttore per inizializzare i repository.
    /// </summary>
    /// <param name="customerRepository">Repository per le entità di tipo Customer.</param>
    /// <param name="orderRepository">Repository per le entità di tipo Order.</param>
    public NorthwindService(IRepository<Customer> customerRepository, IRepository<Order> orderRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Aggiunge un nuovo cliente al database.
    /// </summary>
    /// <param name="customer">Cliente da aggiungere.</param>
    /// <returns>Task di completamento.</returns>
    public async Task AddCustomerAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
    }

    /// <summary>
    /// Ottiene tutti i clienti dal database.
    /// </summary>
    /// <returns>Lista di clienti.</returns>
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    /// <summary>
    /// Modifica un cliente esistente nel database.
    /// </summary>
    /// <param name="customer">Cliente da modificare.</param>
    /// <returns>Task di completamento.</returns>
    public async Task UpdateCustomerAsync(Customer customer)
    {
        await _customerRepository.UpdateAsync(customer);
    }

    /// <summary>
    /// Elimina un cliente dal database.
    /// </summary>
    /// <param name="id">ID del cliente da eliminare.</param>
    /// <returns>Task di completamento.</returns>
    public async Task DeleteCustomerAsync(object id)
    {
        await _customerRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Aggiunge un nuovo ordine al database.
    /// </summary>
    /// <param name="order">Ordine da aggiungere.</param>
    /// <returns>Task di completamento.</returns>
    public async Task AddOrderAsync(Order order)
    {
        await _orderRepository.AddAsync(order);
    }

    /// <summary>
    /// Ottiene tutti gli ordini dal database.
    /// </summary>
    /// <returns>Lista di ordini.</returns>
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    /// <summary>
    /// Modifica un ordine esistente nel database.
    /// </summary>
    /// <param name="order">Ordine da modificare.</param>
    /// <returns>Task di completamento.</returns>
    public async Task UpdateOrderAsync(Order order)
    {
        await _orderRepository.UpdateAsync(order);
    }

    /// <summary>
    /// Elimina un ordine dal database.
    /// </summary>
    /// <param name="id">ID dell'ordine da eliminare.</param>
    /// <returns>Task di completamento.</returns>
    public async Task DeleteOrderAsync(object id)
    {
        await _orderRepository.DeleteAsync(id);
    }
}

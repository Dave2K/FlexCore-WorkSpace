using FlexCore.Northwind;

var northwindService = new NorthwindService(
    new EfRepository<Customer>(new DbContext()),
    new EfRepository<Order>(new DbContext())
);

Console.WriteLine("1. Aggiungi cliente");
Console.WriteLine("2. Visualizza clienti");
Console.WriteLine("3. Modifica cliente");
Console.WriteLine("4. Elimina cliente");
Console.WriteLine("5. Aggiungi ordine");
Console.WriteLine("6. Visualizza ordini");
Console.WriteLine("7. Modifica ordine");
Console.WriteLine("8. Elimina ordine");

var choice = Console.ReadLine();
switch (choice)
{
    case "1":
        // Aggiungi un cliente
        var customer = new Customer { /* Set properties */ };
        await northwindService.AddCustomerAsync(customer);
        break;

    case "2":
        var customers = await northwindService.GetAllCustomersAsync();
        foreach (var c in customers) Console.WriteLine(c.Name);
        break;

    // Altri casi per modifica, eliminazione, etc.
    default:
        Console.WriteLine("Scelta non valida");
        break;
}

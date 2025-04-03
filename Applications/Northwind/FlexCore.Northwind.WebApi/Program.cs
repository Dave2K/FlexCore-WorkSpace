//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();



using Microsoft.EntityFrameworkCore;
using FlexCore.Northwind;

var builder = WebApplication.CreateBuilder(args);

// Leggi la configurazione per scegliere la modalità
var config = builder.Configuration;
var applicationMode = config.GetValue<string>("ApplicationMode");

// Leggi la connection string dal file di configurazione
var connectionString = config.GetConnectionString("NorthwindDb");

if (applicationMode == "WebApi")
{
    // Aggiungi il servizio DbContext con la connection string
    builder.Services.AddDbContext<DbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddScoped<IRepository<Customer>, EfRepository<Customer>>();
    builder.Services.AddScoped<IRepository<Order>, EfRepository<Order>>();
    builder.Services.AddScoped<NorthwindService>();
    builder.Services.AddControllers();

    var app = builder.Build();
    app.MapControllers();
    app.Run();
}
else if (applicationMode == "ConsoleApp")
{
    // Esegui la Console Application
    var northwindService = new NorthwindService(
        new EfRepository<Customer>(new DbContext()),
        new EfRepository<Order>(new DbContext())
    );

    Console.WriteLine("1. Aggiungi cliente");
    Console.WriteLine("2. Visualizza clienti");
    // Altri comandi per Console
    var choice = Console.ReadLine();
    // Gestisci la logica della Console Application
    Console.WriteLine("Modo Console avviato");
}
else
{
    Console.WriteLine("Modalità non valida. Impostare 'ApplicationMode' su 'WebApi' o 'ConsoleApp'");
}

using BikeStore.Application.Interfaces;
using BikeStore.Infrastructure.Data;
using BikeStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<BikeStoreDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BikeStoreConnection")
    ));

// Inyección de Dependencias
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>(); // <-- ESTA LÍNEA ES LA QUE FALTA REGISTRAR

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
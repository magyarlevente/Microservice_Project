using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Common.Repositories;
using Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// --- SERVICES ---

// 1. Controllerek engedélyezése
builder.Services.AddControllers();

// 2. Swagger generátor hozzáadása (EZ HIÁNYZOTT a felülethez)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Adatbázis kapcsolat
builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ProductDbContext>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// --- PIPELINE ---

// 4. Swagger UI bekapcsolása (Csak fejlesztői módban)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Ez generálja a JSON-t
    app.UseSwaggerUI(); // Ez generálja a weboldalt (HTML/CSS)
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
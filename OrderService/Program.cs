using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using Common.Interfaces;
using Common.Repositories;
using Microsoft.Extensions.Http.Resilience;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<OrderDbContext>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddHttpClient<OrderService.Controllers.OrdersController>(client => 
    {
        client.BaseAddress = new Uri("http://localhost:5235/");
    })
    .AddStandardResilienceHandler(options => 
    {
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(3);
        options.Retry.MaxRetryAttempts = 3;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
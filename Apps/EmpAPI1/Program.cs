using Menu.Domain.Interfaces;
using Menu.Infrastructure;
using Menu.REST.Mapping;
using Menu.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AspNetCoreRateLimit;
using System.Threading.RateLimiting;


#if ApiConventions
using Microsoft.AspNetCore.Mvc;
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
#endif

var builder = WebApplication.CreateBuilder(args);

// Registering AutoMapper mapping profile
builder.Services.AddAutoMapper(typeof(Menu.REST.Mapping.MappingConfig));

// Registering Repositories
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Add DbContext 
builder.Services.AddDbContext<MenuDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MenuDbContext>();

// rate limit
//builder.Services.AddOptions();
//builder.Services.AddMemoryCache();
//builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
//builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
//builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 30,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseIpRateLimiting();
app.UseRateLimiter();


app.MapHealthChecks("/health");

app.MapControllers();

app.Run();

using core7_redis.Models;
using core7_redis.Cache;
using core7_redis.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// REDIS IN-MEMORY CACHE

// REDIS DISTRIBUTED CACHED
// builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllers();

builder.Services.AddStackExchangeRedisCache(options => 
{
    options.Configuration = builder.Configuration.GetSection("RedisConnection").GetValue<string>("Configuration");
    options.InstanceName = builder.Configuration.GetSection("RedisConnection").GetValue<string>("InstanceName");
});

builder.Services.AddScoped < IUserService, UserService > ();
builder.Services.AddScoped < ICacheService, CacheService > ();
builder.Services.AddDbContext < DbContextClass > (options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
));


// builder.Services.AddDistributedSqlServerCache(options =>
// {
//     options.ConnectionString = builder.Configuration.GetConnectionString(
//         "DistCache_ConnectionString");
//     options.SchemaName = "dbo";
//     options.TableName = "TestCache";
// });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// app.Lifetime.ApplicationStarted.Register(() =>
// {
//     var currentTimeUTC = DateTime.UtcNow.ToString();
//     byte[] encodedCurrentTimeUTC = System.Text.Encoding.UTF8.GetBytes(currentTimeUTC);
//     var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20));
//     app.Services.GetService<IDistributedCache>().Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
// });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

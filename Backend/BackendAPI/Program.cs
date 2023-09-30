using BackendAPI;
using InstantAPIs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("SqlConnectionString");

builder.Services.AddDbContext<ApartamentAdsDbContext>(options => options.UseSqlServer(connectionString));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddInstantAPIs();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.MapInstantAPIs<ApartamentAdsDbContext>();

#region Object



#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
using BanqueBack.Helpers;
using BanqueBack.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

{
    var services = builder.Services;
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
          builder =>
          {
              builder.WithOrigins("http://localhost:3000",
                                  "*")
                                     .AllowAnyHeader()
                                     .AllowAnyMethod();
          });
    });
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

}

// Add services to the container.
builder.Services.AddDbContext<BanqueContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DevConnectionPostGreSQL"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//Scaffold-DbContext "Server=localhost;Port=5432;Database=Banque;User Id=postgres;Password=root" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models
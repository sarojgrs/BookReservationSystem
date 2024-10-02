using BookReservationSystem.Domain.Context;
using BookReservationSystem.WebApi.Middleware;
using BookReservationSystem.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BookReservationSystem.WebApi.Filters;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureService(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();

//Configure API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0); // Set the default version here
    options.ReportApiVersions = true;
});

builder.Services.AddDbContext<BookDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("BookDbConnection"),
            b => b.MigrationsAssembly("BookReservationSystem.Domain")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Reservation API", Version = "v1.0" });
    c.SchemaFilter<DefaultValueSchemaFilter>(); // Register the schema filter here
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Register the custom error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Reservation API v1.0");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

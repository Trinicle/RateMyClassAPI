using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RateMyClass.API.DbContexts;
using RateMyClass.API.Entities;
using System.Formats.Asn1;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Install SQLLite/SQL Server extension for SQL viewer
// Right click Universities table in the UniversityInfo.db database and click on CSV import for fast data import.
builder.Services.AddDbContext<UniversityInfoContext>(dbContextoptions
    => dbContextoptions.UseSqlite("Data Source=UniversityInfo.db"));

var app = builder.Build();

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

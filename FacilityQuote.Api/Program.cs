using System.Text.Json.Serialization;
using FacilityQuote.Application.Requests;
using FacilityQuote.Application.Services;
using FacilityQuote.Infrastructure.Persistence;
using FacilityQuote.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<ServicesService>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

builder.Services.AddDbContext<FacilityQuoteDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("FacilityQuote"))
    .UseSeeding((context, _) =>
    {
        FacilityQuoteDataSeeder.Seed(
            (FacilityQuoteDbContext)context);
    }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();